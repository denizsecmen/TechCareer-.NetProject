using E_commerce_website_Techcareer.Database;
using E_commerce_website_Techcareer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace E_commerce_website_Techcareer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Lock()
        {
            HttpContext.Session.SetString("Lock", "123456");
            return RedirectToAction("Cart");
        }
        public IActionResult Unlock()
        {
            HttpContext.Session.Remove("Lock");
            return RedirectToAction("Cart");
        }
        public IActionResult Index()
        {
            return View("Index","_layout");
        }
        public IActionResult Propupd(Updateitem tr)
        {
            DBtech dbtech = new DBtech();
            string extension = Path.GetExtension(tr.image.FileName);
            Products pro = dbtech.Products.Where(p => p.ItemName.Equals(tr.updatedEl)).Single();
            if(pro!=null)
            {
                Products gh = new Products();
                string hashdf = tr.image.FileName.GetHashCode().ToString() + extension;
                gh.ItemName = tr.Prodname;
                gh.Price = tr.price;
                gh.category = tr.category;
                gh.Imageroad = hashdf;
                gh.ItemNo = tr.image.FileName.GetHashCode();
                gh.Stok = tr.stock;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Prodimage", hashdf);
                var stream = new FileStream(filePath, FileMode.Create);
                tr.image.CopyTo(stream);
                dbtech.Products.Add(gh);
                dbtech.Products.Remove(pro);
                dbtech.SaveChanges();


            }
            return RedirectToAction("Profile");
        }

        [Obsolete]
        public IActionResult DeleteCart()
        {
            DBtech dbtech = new DBtech();
            string cartitem = Request.Query["item"];
            ViewBag.check = cartitem;
            string itemid = dbtech.Products.Where(p => p.ItemName.Equals(cartitem)).Select(p =>p.ItemNo.ToString()).Single();
            Middle itemDeleted = dbtech.Fiddle.Where(p => p.prodID.Equals(itemid)).FirstOrDefault();
            dbtech.Fiddle.Remove(itemDeleted);
            dbtech.SaveChanges();
            return RedirectToAction("Cart");
        }
        public IActionResult Shopsearch()
        {
            DBtech dbtech = new DBtech();
            string quer = Request.Query["ad"];
            string buy = Request.Query["Buy"];
            if (quer!=null) { 
            IQueryable<Products> searchlist = from p in dbtech.Products
                                              select new Products()
                                              {
                                                  Price = p.Price,
                                                  ItemName = p.ItemName,
                                                  category = p.category,
                                                  Imageroad = p.Imageroad,
                                                  Stok = p.Stok
                                              
                                              };
            searchlist = searchlist.Where(p => p.category.Equals(quer));
            return View("Shop",searchlist);
            }
            if(buy!=null)
            {
                IQueryable<Products> Prod = dbtech.Products.Where(p => p.ItemName.Equals(buy));
                if (Prod.Count() > 0)
                {
                    IQueryable<Users> userIds = from p in dbtech.User select new Users() { Name = p.Name,Id = p.Id };
                    Users userId = userIds.Where(p => p.Name == HttpContext.Session.GetString("UserAuth")).Single();
                    int prodid = dbtech.Products.Where(p => p.ItemName == buy).Select(p => p.ItemNo).Single();
                    Middle Chlist = new Middle();
                    Chlist.UserId = userId.Id.ToString();
                    Chlist.prodID = prodid.ToString();
                    Chlist.transId = userId.GetHashCode().ToString();
                    dbtech.Fiddle.Add(Chlist);
                    dbtech.SaveChanges();

                }
                return RedirectToAction("Shop");
            }
            return RedirectToAction("Shop");
        }
        public IActionResult Shop()
        {
            DBtech dbtech = new DBtech();
            IQueryable<object> prodlist = from p in dbtech.Products select new Products() { Price = p.Price,ItemName=p.ItemName,category=p.category,Imageroad=p.Imageroad,Stok=p.Stok};
            return View(prodlist);
        }
        public IActionResult ShopDetail()
        {
            return View();
        }
        public IActionResult Contract()
        {
            return View();
        }
        public IActionResult Cart()
        {
            DBtech db = new DBtech();
            Users id =db.User.Where(p=>p.Name.Equals(HttpContext.Session.GetString("UserAuth"))).SingleOrDefault();
            if(id==null)
            {
                return View("Cart", null);
            }
            IQueryable<CartView> middle = (from p in db.Products
                                         join c in db.Fiddle on
                                        p.ItemNo.ToString() equals c.prodID
                                         where id.Id.ToString().Equals(c.UserId)
                                         select new CartView()
                                         {
                                             name = p.ItemName,
                                             userId = c.UserId,
                                             img = p.Imageroad,
                                             price=p.Price,
                                         });
            return View(middle);
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CloseSession()
        {
            if(HttpContext.Session.GetString("UserAuth")!=null)
            {
                HttpContext.Session.Remove("UserAuth");
            }
            if(HttpContext.Session.GetString("Admin")!=null)
            {
                HttpContext.Session.Remove("Admin");
            }
            return View("Index");
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel gmodel)
        {
            try
            {
                DBtech dBtech = new DBtech();
                IQueryable<Users> user = dBtech.User.Where(p => p.Email == gmodel.name);
                if (user.Count() == 0)
                {
                    ViewBag.mes = "Kullanıcı Bulunamadı";
                    return View();
                }
                Users pswuser = user.Where(p => p.Pass.Equals(gmodel.password)).SingleOrDefault();
                if (pswuser == null)
                {
                    ViewBag.mes = "Şifre yanlış.";
                    return View();
                }
                else
                {
                    if (HttpContext.Session.GetString("Admin") != null)
                    {
                        HttpContext.Session.SetString("Admin", null);
                    }
                    HttpContext.Session.SetString("UserAuth", pswuser.Name);
                    return View("Index","_Layout");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Prpofiledel(Itemdelmodel item)
        {
            DBtech Dbt = new DBtech();
            Products test = Dbt.Products.Where(p => p.ItemName.Equals(item.itlist)).SingleOrDefault();
            if(test==null)
            {
                ViewBag.messa = "Böyle bütün veritabanında yok.";
                return RedirectToAction("Profile");
            }
            else
            {
                Dbt.Products.Remove(test);
                Dbt.SaveChanges();

            }
            return RedirectToAction("Profile");
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                DBtech Dbt = new DBtech();
                List<string> test = Dbt.Products.Select(p => p.ItemName).ToList();
                return View(test);
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult Prpofile(ProductModel prmodel)
        {
            try
            {
                if ( prmodel.Prodname==null||prmodel.stock==null||prmodel.price==null|| prmodel.category==null)
                {
                    ViewBag.general = "Lütfen Bütün alanları doldurunuz.";

                    return RedirectToAction("Profile");
                }
                if(prmodel.price<0 || prmodel.stock<0)
                {
                    ViewBag.general = "Stok ve ücret 0 dan küçük olamaz";
                    return RedirectToAction("Profile");
                }
                else if(prmodel.image==null)
                {
                    ViewBag.general = "Lütfen Bütün alanları doldurunuz.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    ViewBag.general = "";
                    string filename = prmodel.image.FileName;
                    string extension = Path.GetExtension(filename);
                    if(extension==".jpg" || extension==".png")
                    {
                        DBtech Dbt = new DBtech();
                        IQueryable<Products> query = Dbt.Products.Where(p => p.ItemName.Equals(prmodel.Prodname));
                        if (query.Count() > 0)
                        {
                            ViewBag.general = "Bu isimde ürün mevcut.";
                            return RedirectToAction("profile");
                        }
                        string hashdf = filename.GetHashCode().ToString() + extension;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Prodimage", hashdf);
                        var stream = new FileStream(filePath, FileMode.Create);
                        prmodel.image.CopyTo(stream);
                        Products product = new Products();
                        product.ItemNo = prmodel.Prodname.GetHashCode();
                        product.ItemName = prmodel.Prodname;
                        product.Stok = prmodel.stock;
                        product.Imageroad = hashdf;
                        product.Price = prmodel.price;
                        product.category = prmodel.category;
                        Dbt.Products.Add(product);
                        Dbt.SaveChanges();
                        return RedirectToAction("Profile");
                    }
                  
                    else
                    {
                        ViewBag.general = "Sadece .jpg ve .png uzantılar kabul edilecektir!";
                        return View("profile");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = "Hata gerçekleşti";
                return View("Profile");
            }
        }
        [HttpPost]
        public IActionResult Adminpost(Adminview adm)
        {
            try
            {
                DBtech Dbt = new DBtech();
                IQueryable<Admin> admins = Dbt.Admins.Where(p => p.AdminName.Equals(adm.name) && p.AdminPass.Equals(adm.password));
                
                if (admins.Count() > 0)
                {
                    if(HttpContext.Session.GetString("UserAuth")!=null)
                    {
                        HttpContext.Session.SetString("UserAuth", null);
                    }
                    HttpContext.Session.SetString("Admin",adm.name);
                    return RedirectToAction("Profile");
                }
                else
                {
                    ViewBag.message = "Bilgiler geçerli değil";
                    return View("Admin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.InnerException;
                return View("Admin");
            }
        }
        [HttpPost]
        public IActionResult KullaniciEkle(UserAddView signedUser)
        {
            try
            {
                if (signedUser.password == null || signedUser.email == null
               || signedUser.cpassword == null || signedUser.name == null)
                {
                    ViewBag.general = "Lütfen bütün boş alanları doldurunuz";
                    return View("SignIn");
                }
                if (signedUser.avatar == null)
                {
                    ViewBag.general = "Lütfen resim ekleyiniz";
                    return View("SignIn");
                }
                bool match = Regex.IsMatch(signedUser.password, "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
                if (!match)
                {
                    ViewBag.general = "Şifre en az 8 karakter içermeli noktlama işareti ve büyük harf sahibi olmalıdır";
                    return View("SignIn");
                }
                else if (!signedUser.password.Equals(signedUser.cpassword))
                {
                    ViewBag.password = "Parolalar aynı olmak zorundadır";
                    return View("SignIn");
                }
                else
                {
                    ViewBag.password = "";
                    string filename = signedUser.avatar.FileName;
                    string fileExtension = Path.GetExtension(filename);
                    if (fileExtension == ".jpg" || fileExtension == ".png")
                    {
                        DBtech dBtech = new DBtech();
                        IQueryable<Users> checklist = dBtech.User.Where(p => p.Email.Equals(signedUser.email));
                        if (checklist.Count() == 0)
                        {
                            string hashedf = (filename.GetHashCode().ToString() + fileExtension);
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", hashedf);
                            var stream = new FileStream(filePath, FileMode.Create);
                            signedUser.avatar.CopyTo(stream);
                            Users user = new Users();
                            user.Email = signedUser.email;
                            user.Name = signedUser.name;
                            user.Pass = signedUser.password;
                            user.Id = signedUser.name.GetHashCode();
                            user.Imageroad = hashedf;
                            dBtech.User.Add(user);
                            dBtech.SaveChanges();
                            return View("SignIn");
                        }
                        else
                        {
                            ViewBag.general = "Aynı emailden olan kullanıcılar olamaz.";
                            return View("SignIn");
                        }
                    }
                    else
                    {
                        ViewBag.general = "Dosya uzantısı .jpg veya .png olmalıdır!";
                        return View("SignIn");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.general = ex.InnerException.ToString();
                return View("SignIn");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new E_commerce_website_Techcareer.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}