using System;
using System.Diagnostics;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;

namespace ConsumerOne.Mobile.Droid
{
    public static class AndroidHelpers
    {
        public static void SetShiftMode(this BottomNavigationView bottomNavigationView, bool enableShiftMode,
            bool enableItemShiftMode)
        {
            try
            {
                var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
                if (menuView == null)
                {
                    Debug.WriteLine("Unable to find BottomNavigationMenuView");
                    return;
                }


                var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");

                shiftMode.Accessible = true;
                shiftMode.SetBoolean(menuView, enableShiftMode);
                shiftMode.Accessible = false;
                shiftMode.Dispose();


                for (var i = 0; i < menuView.ChildCount; i++)
                {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;

                    item.SetShifting(enableItemShiftMode);
                    item.SetChecked(item.ItemData.IsChecked);
                }

                menuView.UpdateMenuView();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }
    }
}