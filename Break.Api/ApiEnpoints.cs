namespace Break.Api;

public static class ApiEnpoints
{
    private const string ApiBase = "api";

    public static class Item
    {
        private const string Base = $"{ApiBase}/item";

        public const string CreateItem = Base;
        public const string GetItem = $"{Base}/{{id:int}}";
        public const string GetAllItems = Base;
        public const string UpdateItem = $"{Base}/{{id:int}}";
        public const string DeleteItem = $"{Base}/{{id:int}}";
    }

    public static class Offer
    {
        private const string Base = $"{ApiBase}/offer";

        public const string CreateOffer = Base;
        public const string GetOffer = $"{Base}/{{id:int}}";
        public const string GetAllOffers = Base;
        public const string UpdateOffer = $"{Base}/{{id:int}}";
        public const string DeleteOffer = $"{Base}/{{id:int}}";
    }

    public static class Sale
    {
        private const string Base = $"{ApiBase}/sale";

        public const string CreateSale = Base;
        public const string GetSale = $"{Base}/{{id:int}}";
        public const string GetAllSales = Base;
        public const string UpdateSale = $"{Base}/{{id:int}}";
        public const string DeleteSale = $"{Base}/{{id:int}}";
        public const string ConfirmSale = $"{Base}/{{id:int}}/confirm";
        public const string CancelSale = $"{Base}/{{id:int}}/cancel";
    }

    public static class Auth
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }
}
