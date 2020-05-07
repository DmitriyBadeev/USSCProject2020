using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class Position : IDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
