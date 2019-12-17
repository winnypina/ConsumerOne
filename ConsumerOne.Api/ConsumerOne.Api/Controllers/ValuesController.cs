using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Services;
using GeoAPI.Geometries;
using Geocoding.Google;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace ConsumerOne.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            foreach (var post in _context.Posts)
            {
                if (post.Latitude.HasValue)
                {
                    post.Location = geometryFactory.CreatePoint(new Coordinate(post.Longitude.Value, post.Latitude.Value));
                }
                else
                {
                    if (!string.IsNullOrEmpty(post.Address))
                    {
                        var geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDGQl3EKegXZb912468WewrdINcDNeC75I" };
                        var coords = await geocoder.GeocodeAsync(post.Address);
                        var coord = coords.FirstOrDefault();
                        if (coord != null)
                        {
                            post.Location = geometryFactory.CreatePoint(new Coordinate(coord.Coordinates.Latitude, coord.Coordinates.Longitude));
                        }
                    }
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("Geolocation")]
        public async Task<ActionResult<IEnumerable<string>>> GetGeolocation()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var posts = _context.Posts.ToList();
            posts.ForEach(n =>
            {
                if (n.Longitude.HasValue)
                {
                    n.Location = geometryFactory.CreatePoint(new Coordinate(n.Longitude.Value, n.Latitude.Value));
                }
            });
            await _context.SaveChangesAsync();
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("Translation")]
        public async Task<ActionResult<IEnumerable<string>>> GetTranslation()
        {
            await new BaseLanguageCreator(_context).Create();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
