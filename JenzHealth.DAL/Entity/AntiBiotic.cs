using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public class AntiBiotic
    {
        public int Id { get; set; }
        public int? OrganismID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("OrganismID")]
        public Organism Organism { get; set; }
    }
}
