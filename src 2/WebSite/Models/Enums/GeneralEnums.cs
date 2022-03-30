namespace WebSite.Models.Enums
{
    public static class AuthorsRoutes
    {
        #region MyRegion

        /// <summary>
        /// HttpPost
        /// </summary>
        public const string CreateAuthor = "api/Authors/Create";

        /// <summary>
        /// HttpGet
        /// </summary>
        public const string GetAuthorList = "api/Authors/Get";

        /// <summary>
        /// HttpGet Get/{Authorid:int}
        /// </summary>
        public const string GetAuthor = "api/Authors/Get/{0}";

        /// <summary>
        /// HttpPut
        /// </summary>
        public const string UpdateAuthor = "api/Authors/Update";

        /// <summary>
        /// HttpDelete Delete/{AuthorId:int}
        /// </summary>
        public const string Delete = "api/Authors/Delete/{0}";

        #endregion

    }

    public static class BooksRoutes
    {
        #region MyRegion

        /// <summary>
        /// HttpPost
        /// </summary>
        public const string CreateBook = "api/Books/Create";

        /// <summary>
        /// HttpGet
        /// </summary>
        public const string GetBookList = "api/Books/Get";

        /// <summary>
        /// HttpGet Get/{BookId:int}
        /// </summary>
        public const string GetBook = "api/Books/Get/{0}";

        /// <summary>
        /// HttpPut
        /// </summary>
        public const string UpdateBook = "api/Books/Update";

        /// <summary>
        /// HttpDelete Delete/{BookId:int}
        /// </summary>
        public const string Delete = "api/Books/Delete/{0}";

        #endregion

    }

    public static class ClientsRoutes
    {
        #region MyRegion

        /// <summary>
        /// HttpPost
        /// </summary>
        public const string CreateClient = "api/Clients/Create";

        /// <summary>
        /// HttpGet
        /// </summary>
        public const string GetClientList = "api/Clients/Get";

        /// <summary>
        /// HttpGet Get/{Clientid:int}
        /// </summary>
        public const string GetClient = "api/Clients/Get/{0}";

        /// <summary>
        /// HttpPut
        /// </summary>
        public const string UpdateClient = "api/Clients/Update";

        /// <summary>
        /// HttpDelete Delete/{Clientid:int}
        /// </summary>
        public const string Delete = "api/Clients/Delete/{0}";

        #endregion

    }

}

