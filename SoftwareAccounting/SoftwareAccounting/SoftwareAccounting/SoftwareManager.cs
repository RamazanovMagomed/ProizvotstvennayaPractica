using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoftwareAccounting
{
    public class SoftwareManager
    {
        private readonly string _filePath = "software.txt";
        private List<SoftwareItem> _softwareItems;

        public SoftwareManager()
        {
            LoadData();
        }

        public List<SoftwareItem> GetAllItems()
        {
            return _softwareItems.OrderBy(x => x.Id).ToList();
        }

        public void AddItem(SoftwareItem item)
        {
            item.Id = _softwareItems.Count > 0 ? _softwareItems.Max(x => x.Id) + 1 : 1;
            _softwareItems.Add(item);
            SaveData();
        }

        public void UpdateItem(SoftwareItem item)
        {
            var existing = _softwareItems.FirstOrDefault(x => x.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                existing.Version = item.Version;
                existing.LicenseType = item.LicenseType;
                existing.Manufacturer = item.Manufacturer;
                existing.PurchaseDate = item.PurchaseDate;
                existing.Cost = item.Cost;
                existing.ResponsiblePerson = item.ResponsiblePerson;
                existing.UserCount = item.UserCount;
                existing.ExpirationDate = item.ExpirationDate;
                existing.Notes = item.Notes;
                SaveData();
            }
        }

        public void DeleteItem(int id)
        {
            var item = _softwareItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _softwareItems.Remove(item);
                SaveData();
            }
        }

        private void LoadData()
        {
            _softwareItems = new List<SoftwareItem>();

            if (File.Exists(_filePath))
            {
                try
                {
                    var lines = File.ReadAllLines(_filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 11)
                        {
                            var item = new SoftwareItem
                            {
                                Id = int.Parse(parts[0]),
                                Name = parts[1],
                                Version = parts[2],
                                LicenseType = parts[3],
                                Manufacturer = parts[4],
                                PurchaseDate = DateTime.Parse(parts[5]),
                                Cost = decimal.Parse(parts[6]),
                                ResponsiblePerson = parts[7],
                                UserCount = int.Parse(parts[8]),
                                ExpirationDate = DateTime.Parse(parts[9]),
                                Notes = parts[10]
                            };
                            _softwareItems.Add(item);
                        }
                    }
                }
                catch
                {
                    _softwareItems = new List<SoftwareItem>();
                }
            }
        }

        private void SaveData()
        {
            var lines = new List<string>();
            foreach (var item in _softwareItems)
            {
                string line = $"{item.Id}|{item.Name}|{item.Version}|{item.LicenseType}|{item.Manufacturer}|{item.PurchaseDate}|{item.Cost}|{item.ResponsiblePerson}|{item.UserCount}|{item.ExpirationDate}|{item.Notes}";
                lines.Add(line);
            }
            File.WriteAllLines(_filePath, lines);
        }
    }
}