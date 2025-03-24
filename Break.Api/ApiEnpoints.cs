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
}
