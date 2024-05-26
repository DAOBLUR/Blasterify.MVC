using Blasterify.Client.Models;
using System.Linq;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class BaseController : Controller
    {
        #region FUNCTIONS
        public bool VerifySession()
        {
            if (Session["ClientUser"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ClientUser GetClientUser()
        {
            return Session["ClientUser"] as ClientUser;
        }

        public void SetCart(Blasterify.Models.Model.PreRentModel cart)
        {
            if (cart != null)
            {
                Session["Cart"] = cart;
            }
        }

        public Blasterify.Models.Model.PreRentModel GetCart()
        {
            if (Session["Cart"] == null)
            {
                return null;
            }

            return Session["Cart"] as Blasterify.Models.Model.PreRentModel;
        }

        public int GetCartCount()
        {
            Blasterify.Models.Model.PreRentModel cart = Session["Cart"] as Blasterify.Models.Model.PreRentModel;

            if (cart == null)
            {
                return 0;
            }
            else
            {
                return cart.PreRentItems.Count;
            }
        }

        #endregion

        #region REQUEST

        [HttpPost]
        public JsonResult GetCartRequest()
        {
            var cart = GetCart();

            return Json(
                new
                {
                    data = cart.PreRentItems.Values.ToList()
                },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpGet]
        public JsonResult GetCartCountRequest()
        {
            var cart = GetCart();

            if (cart == null)
            {

                return Json(
                    new Result(
                        false,
                        new
                        {
                            Count = 0
                        },
                        "Error"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                return Json(
                    new Result(
                        true,
                        new
                        {
                            Count = cart.PreRentItems != null ? cart.PreRentItems.Count : 0
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        #endregion
    }
}