
  Reference: http://afana.me/post/aspnet-mvc-internationalization.aspx

	1.Create MVC5 project
	
	2. Create Class Project Resources (add some: Resources.[language].resx file)
	
	3. Instantiate Class Project in MVC5 project
	
	MVC5 Files:
	
	@{
		ViewBag.Title = "Home Page";
		var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();    
	}
    <h1>@Resources.[attribute from res file]</h1>
	
	
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture = CultureHelper.GetDefaultCulture(), controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
	
	public static class CultureHelper
    {
        // Valid cultures
        private static readonly List<string> _validCultures = new List<string> { "fr", "fr-FR", "fr-BE", "fr-CA", "fr-LU", "fr-MC", "fr-CH", "en", "en-US", "en-AU", "en-BZ", "en-CA", "en-CB","en-IE","en-JM","en-NZ","en-PH","en-ZA","en-TT","en-GB","en-US","en-ZW", "pt", "pt-BR" };

        // Include ONLY cultures you are implementing        
        private static readonly List<string> _cultures = new List<string> {"en", "fr", "pt" };
        /// <summary>
        /// Returns true if the language is a right-to-left language. Otherwise, false.
        /// </summary>
        public static bool IsRighToLeft()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        }
        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            // make sure it's not null
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            // make sure it is a valid culture first            
            if (_validCultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                return GetDefaultCulture(); // return Default culture if it is invalid

            // if it is implemented, accept it            
            if (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
                return name; // accept it

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            var n = GetNeutralCulture(name);

            foreach (var c in _cultures)
                if (c.StartsWith(n))
                    return c;
            // else 
            // It is not implemented
            return GetDefaultCulture(); // return Default culture as no match found

        }

        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures[0]; // return Default culture
        }
        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }
        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-")) return name;

            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }
    }
	
	 public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //string cultureName = null;
            string cultureName = RouteData.Values["culture"] as string;

            // Attempt to read the culture cookie from Request
            //HttpCookie cultureCookie = Request.Cookies["_culture"];

            //if (cultureCookie != null)
            //{
            //    cultureName = cultureCookie.Value;
            //}
            //else
            //{
            // obtain it from HTTP header AcceptLanguages
            //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;
            //}

            // Attempt to read the culture cookie from Request
            if (cultureName == null)
            {
                // obtain it from HTTP header AcceptLanguages
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; 
            }

            // Validate culture name            
            cultureName = CultureHelper.GetImplementedCulture(cultureName);


            if (RouteData.Values["culture"] as string != cultureName)
            {
                // Force a valid culture in the URL
                RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

                // Redirect user
                Response.RedirectToRoute(RouteData.Values);
            }


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
	
	//Inherith BaseController on HomeController and add this method: SetCulture
	
	public ActionResult SetCulture(string culture)
	{
		// Validate input
		culture = CultureHelper.GetImplementedCulture(culture);

		RouteData.Values["culture"] = culture;  // set culture

		// Save culture in a cookie
		//HttpCookie cookie = Request.Cookies["_culture"];
		//if (cookie != null)
		//    cookie.Value = culture;   // update cookie value
		//else
		//{
		//    cookie = new HttpCookie("_culture");
		//    cookie.Value = culture;
		//    cookie.Expires = DateTime.Now.AddYears(1);
		//}
		//Response.Cookies.Add(cookie);
		
		return RedirectToAction("Index");
	}
	
	//Views/Web.config:	
  <system.web.webPages.razor>
    <host factoryType="System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" />
    <pages pageBaseType="System.Web.Mvc.WebViewPage">
      <namespaces>
        <add namespace="System.Web.Mvc" />
        <add namespace="System.Web.Mvc.Ajax" />
        <add namespace="System.Web.Mvc.Html" />
        <add namespace="System.Web.Optimization"/>
        <add namespace="System.Web.Routing" />
        <add namespace="MVC5Internacionalization" />
      --->    #### <add namespace="Resources"/> ##### INCLUDE
      </namespaces>
    </pages>
  </system.web.webPages.razor>	
  
  //Change _Layout.cshtml
  <html lang="@MVC5Internacionalization.Helpers.CultureHelper.GetCurrentNeutralCulture()">
		
		
	
		
	
	
	
	
