using System;

namespace SoftwareAccounting
{
    public class SoftwareItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string LicenseType { get; set; }
        public string Manufacturer { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Cost { get; set; }
        public string ResponsiblePerson { get; set; }
        public int UserCount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }

        public SoftwareItem()
        {
            PurchaseDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(1);
            UserCount = 1;
        }
    }
}