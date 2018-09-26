using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace University.Extensions
{
    public static class ICollectionExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this ICollection<T> roles, int selectedValue)
        {
            return from role in roles
                   select new SelectListItem
                   {
                       Text = role.GetPropertyValue("RoleName"),
                       Value = role.GetPropertyValue("RoleId"),
                       Selected = role.GetPropertyValue("RoleId").Equals(selectedValue.ToString())
                   };
        }
    }
}