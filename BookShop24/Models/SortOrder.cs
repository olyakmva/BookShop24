namespace BookShop24.Models
{
    public enum SortOrder
    {
        NameAsc,    // по названию по возрастанию
        NameDesc,   // по названию по убыванию
        PriceAsc, // по цене по возрастанию
        PriceDesc,    // по цене по убыванию
        CategoryAsc, // по жанру по возрастанию
        CategoryDesc, // по жанру по убыванию
        NumberAsc,    // по количеству по возрастанию
        NumberDesc,   // по количеству по убыванию
        AuthorAsc, // по автору по возрастанию
        AuthorDesc,    // по автору по убыванию
        YearAsc,
        YearDesc      // по году
    }
}
