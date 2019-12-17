using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Models;
using ConsumerOne.Api.ViewModels;
using GeoAPI.Geometries;
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using NetTopologySuite;
using ZXing;
using ZXing.Common;

namespace ConsumerOne.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PostController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("{term}/qrcode")]
        public async Task<ActionResult> GetQRCode(string term)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions { Height = 300, Width = 300 }
            };

            var pixelData = qrWriter.Write(term);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package

            var ms = new MemoryStream();
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                        pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                // save to stream as PNG
                bitmap.Save(ms, ImageFormat.Jpeg);
                ms.Position = 0;
            }

            return File(ms, "image/jpeg");
        }

        [HttpPost]
        [Authorize]
        [Route("search/paged/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetSearchPaged([FromBody]SearchViewModel request, int pageNumber)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
             var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var query = _dbContext.Posts.Include(n=>n.AppUser).Include(n => n.PostLikes).Include(n => n.PostMedias).Where(n=>!n.IsPaused).AsQueryable();

            if (!string.IsNullOrEmpty(request.Term))
            {
                switch (request.SearchType)
                {
                    case SearchType.Posts:
                        query = query.Where(n => n.Title.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase));
                        break;
                    case SearchType.Providers:
                        query = query.Where(n => n.AppUser.Name.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            

            Coordinate orderDistance = null;

            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                orderDistance = new Coordinate(request.Longitude.Value, request.Latitude.Value);
                query = query.Where(n =>
                    n.Location.IsWithinDistance(
                        geometryFactory.CreatePoint(orderDistance), (request.Distance * 1000)));
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Address))
                {
                    var geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDGQl3EKegXZb912468WewrdINcDNeC75I" };
                    var coords = await geocoder.GeocodeAsync(request.Address);
                    var coord = coords.FirstOrDefault();
                    orderDistance = new Coordinate(coord.Coordinates.Longitude, coord.Coordinates.Latitude);
                    if (coord != null)
                    {
                        query = query.Where(n => n.Location.Distance(geometryFactory.CreatePoint(orderDistance)) < (request.Distance * 1000));
                    }
                }
            }

            switch (request.OrderType)
            {
                case OrderType.Date:
                    query = query.OrderByDescending(n => n.PublishDate);
                    break;
                case OrderType.Distance:
                    query = query.OrderBy(n => n.Location.Distance(geometryFactory.CreatePoint(orderDistance)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var posts = await query.Skip(pageNumber * 5).Take(5).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Value = post.Price ?? 0,
                Tags = post.Tags,
                Video = post.PostMedias.SingleOrDefault(n => n.MediaType == MediaType.Video)?.Id.ToString(),
                Longitude = post.Longitude,
                Description = post.Description,
                Title = post.Title,
                Latitude = post.Latitude,
                Images = post.PostMedias.Where(n => n.MediaType == MediaType.Image).Select(n => n.Id).ToList(),
                Address = post.Address,
                IsLikedByUser = post.PostLikes.Any(n => n.AppUserId == user.Id),
                Currency = post.Currency,
                LikeCount = post.LikeCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                StoreLink = post.StoreLink,
                PublishDate = post.PublishDate
            }));
        }


        [HttpGet]
        [Authorize]
        [Route("paged/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPaged(int pageNumber)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var posts = await _dbContext.Posts.Include(n => n.AppUser).Include(n=>n.PostLikes).Include(n => n.PostMedias).Where(n=>!n.IsPaused)
                .OrderByDescending(n => n.PublishDate).Skip(pageNumber * 5).Take(5).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Value = post.Price ?? 0,
                Tags = post.Tags,
                Video = post.PostMedias.SingleOrDefault(n => n.MediaType == MediaType.Video)?.Id.ToString(),
                Longitude = post.Longitude,
                Description = post.Description,
                Title = post.Title,
                Latitude = post.Latitude,
                Images = post.PostMedias.Where(n => n.MediaType == MediaType.Image).Select(n => n.Id).ToList(),
                Address = post.Address,
                IsLikedByUser = post.PostLikes.Any(n=>n.AppUserId == user.Id),
                Currency = post.Currency,
                LikeCount = post.LikeCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                StoreLink = post.StoreLink,
                PublishDate = post.PublishDate
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("user/{userId}/paged/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPagedByUser(string userId, int pageNumber)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var posts = await _dbContext.Posts.Include(n => n.AppUser).Include(n => n.PostLikes).Include(n => n.PostMedias).Where(n => !n.IsPaused && n.AppUserId == userId)
                .OrderByDescending(n => n.PublishDate).Skip(pageNumber * 5).Take(5).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Value = post.Price ?? 0,
                Tags = post.Tags,
                Video = post.PostMedias.SingleOrDefault(n => n.MediaType == MediaType.Video)?.Id.ToString(),
                Longitude = post.Longitude,
                Description = post.Description,
                Title = post.Title,
                Latitude = post.Latitude,
                Images = post.PostMedias.Where(n => n.MediaType == MediaType.Image).Select(n => n.Id).ToList(),
                Address = post.Address,
                IsLikedByUser = post.PostLikes.Any(n => n.AppUserId == user.Id),
                Currency = post.Currency,
                LikeCount = post.LikeCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                StoreLink = post.StoreLink,
                PublishDate = post.PublishDate
            }));
        }


        [HttpGet]
        [Authorize]
        [Route("comment/{postId:guid}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetComments(Guid postId)
        {
            var comments = await _dbContext.PostComments.Include(n => n.AppUser).Where(n=>n.PostId == postId)
                .OrderByDescending(n => n.PublishDate).ToListAsync();

            return new OkObjectResult(comments.Select(comment => new PostCommentViewModel
            {
                Id = comment.Id,
                Description = comment.Description,
                PublishDate = comment.PublishDate,
                AppUserId = comment.AppUserId,
                PostId = comment.PostId,
                AppUsername = comment.AppUser.Name
            }));
        }

        [Authorize]
        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult> PostComment([FromBody] PostCommentViewModel request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == request.PostId);

            if (post == null) return new NotFoundResult();

            var comment = new PostComment
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                Description = request.Description,
                PublishDate = DateTime.Now,
                PostId = request.PostId
            };
            await _dbContext.PostComments.AddAsync(comment);
            
            await _dbContext.SaveChangesAsync();
            request.Id = post.Id;
            return new OkObjectResult(request);
        }

        [Authorize]
        [HttpPost]
        [Route("comment/{id:guid}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var postComment = await _dbContext.PostComments.SingleOrDefaultAsync(n => n.Id == id);

            if (postComment == null) return new NotFoundResult();

            _dbContext.PostComments.Remove(postComment);

            await _dbContext.SaveChangesAsync();
            
            return new OkResult();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostViewModel request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var post = new Post
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Price = request.Value,
                Title = request.Title,
                Tags = request.Tags,
                Address = request.Address,
                Currency = request.Currency,
                StoreLink = request.StoreLink,
                PublishDate = DateTime.Now
            };
            await _dbContext.Posts.AddAsync(post);

            if (request.Images != null)
            {
                foreach (var requestImage in request.Images)
                {
                    var postImage = new PostMedia
                    {
                        Id = requestImage,
                        PostId = post.Id,
                        MediaType = MediaType.Image
                    };
                    await _dbContext.PostMedias.AddAsync(postImage);
                }
            }
            

            if (request.Video != null)
            {
                var postImage = new PostMedia
                {
                    Id = Guid.Parse(request.Video),
                    PostId = post.Id,
                    MediaType = MediaType.Video
                };
                await _dbContext.PostMedias.AddAsync(postImage);
            }

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            if (request.Latitude.HasValue)
            {
                post.Location = geometryFactory.CreatePoint(new Coordinate(post.Longitude.Value, post.Latitude.Value));
            }
            else
            {
                if(!string.IsNullOrEmpty(request.Address))
                {
                    var geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDGQl3EKegXZb912468WewrdINcDNeC75I" };
                    var coords = await geocoder.GeocodeAsync(request.Address);
                    var coord = coords.FirstOrDefault();
                    if (coord != null)
                    {
                        post.Location = geometryFactory.CreatePoint(new Coordinate(coord.Coordinates.Latitude, coord.Coordinates.Longitude));
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            request.Id = post.Id;
            return new OkObjectResult(request);
        }

        [Authorize]
        [HttpPost]
        [Route("like/{id:guid}")]
        public async Task<ActionResult> Like(Guid id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == id);

            if (post == null) return new NotFoundResult();

            var currentLike = _dbContext.PostLikes.SingleOrDefault(n => n.AppUserId == user.Id && n.PostId == id);

            if (currentLike == null)
            {
                if (post.PostLikes == null)
                {
                    post.PostLikes = new List<PostLike>();
                }
                post.LikeCount = post.LikeCount + 1;
                post.PostLikes.Add(new PostLike
                {
                    AppUserId = user.Id,
                    PostId = post.Id
                });
            }
            else
            {
                post.LikeCount = post.LikeCount -= 1;
                post.PostLikes.Remove(currentLike);
            }

            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [Authorize]
        [HttpPost]
        [Route("pause/{id:guid}")]
        public async Task<ActionResult> Pause(Guid id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == id && n.AppUserId == user.Id);

            if (post == null) return new NotFoundResult();

            post.IsPaused = !post.IsPaused;

            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == id && n.AppUserId == user.Id);

            if (post == null) return new NotFoundResult();

            _dbContext.Posts.Remove(post);

            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
    }

    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}