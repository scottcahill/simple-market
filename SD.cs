namespace SmWeb.Utils
{
    public static class SD
    {

        public const string BaseImageUrl = "https://smimageblob.blob.core.windows.net/images/";

        public const string ApiBaseUrl = "https://localhost:5001/";

        // Members ----------------
        public const string MemberApiPath = ApiBaseUrl + "api/member";
        public const string MemberByEmailEp = MemberApiPath +"/email/";
        public const string MemberRegisterEp = MemberApiPath + "/register/";
        public const string MemberUpdtByIdEp = MemberApiPath + "/";
        public const string MemberAuthEp = MemberApiPath + "/authenticate/";

        // Category ----------------
        public const string CategoryApiPath = ApiBaseUrl + "api/category/";

        // Product ----------------
        public const string ProductApiPath = ApiBaseUrl + "api/product/";

        // Stores ----------------
        public const string StoreApiPath = ApiBaseUrl + "api/store/";
        public const string StoreByBusnameEp= StoreApiPath + "businessname/";
        public const string StoreBusnameUniqueEp = StoreApiPath + "name/";
        public const string StoreByVendorIdEp = StoreApiPath + "vendorId/";

        // Shopping Cart ----------------
        public const string ShopCartApiPath = ApiBaseUrl + "api/shopcart/";

        //  Security ------------------
        public const string JwtToken = "JWTok";
        public const string AdminRole = "Admin";
        public const string MemberRole = "Member";
        public const string VendorRole = "Vendor";
        public const string Shopcart = "SCart";

        //  Session state ------------------
        public const string SessionRole = "SesRole";
        public const string SessionMemType = "Sesmt";
        public const string SessionMemId = "Sesmid";
        public const string SessionBusname = "Sesbn";
        public const string SessionEmail = "SesEm";

        //  Session member type [enum conversion] ------------------
        //  Enum is integer but Session storage requires string
        public const string MemTypePublic = "Public";
        public const string MemTypeMember = "Member";
        public const string MemTypeVendor = "Vendor";
        public const string MemTypeTrader = "Trader";
        public const string MemTypeSeeder = "Seeder";


        //  Stripe -------------------------
        public const string SMSTRIPEVendorPlanId = "sm-vendor";   // yearly 14.95
        public const string PLAN_JAN13_2019 = "plan_EKs4z9tDl0Hsp1";
        public const string VENFEE = "$14.95";
        public const long VENFEEPENNIES = 1495;
        //  moved to appsettings / StripeOptions.cs
        //public const string StripeTestPubKey = "pk_test_uxfnEYi7aCey3mW8kkblvvod";
        //public const string StripeTestSecretKey = "sk_test_1izlCbdFnRDdotmCj92aIduU";
        //public const string StripeSimpleMarketPlan = "plan_DhfGmPa4vqcVdH";



    }
}
