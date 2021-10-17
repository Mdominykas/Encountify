using System;
using System.Collections.Generic;

namespace Encountify.Models
{
    [Flags]
    public enum Category : int
    {
        None = 0,
        Aquaria = 1,
        Beach = 2,
        AmusementPark = 4,
        BotanicalGarden = 8,
        Park = 16,
        Zoo = 32,
        Casino = 64,
        Cathedral = 1 << 7,
        Castle = 1 << 8,
        Church = 1 << 9,
        Fort = 1 << 10,
        Memorial = 1 << 11,
        Monument = 1 << 12,
        Museum = 1 << 13,
        Resort = 1 << 14,
        SportFacility = 1 << 15,
        Street = 1 << 16,
        Other = 1 << 17,
    };
    
    public static class CategoryConverter
    {
        public static string ConvertCategoryToString(Category category)
        {
            List<string> answer = new List<string>();
            foreach(int id in Enum.GetValues(typeof(Category)))
            {
                if (((int)category & id) != 0)
                    answer.Add(Enum.GetName(typeof(Category), id));
            }
            return string.Join(", ", answer.ToArray());
        }

        // not all have been added
        // multiple categories are not supported
        public static Category ConvertStringToCategory(string name)
        {
            switch (name)
            {
                case "Amusement park":
                    return Category.AmusementPark;

                case "Aquaria":
                    return Category.Aquaria;

                case "Beach":
                    return Category.Beach;

                case "Botanical garden":
                    return Category.BotanicalGarden;

                case "Casino":
                    return Category.Casino;

                case "Castle":
                    return Category.Castle;

                case "Cathedral":
                    return Category.Cathedral;

                case "Fort":
                    return Category.Fort;

                case "Memorial":
                    return Category.Memorial;

                case "Monument":
                    return Category.Monument;

                case "Museum":
                    return Category.Museum;

                default:
                    return Category.None;
            }
        }
    }
}
