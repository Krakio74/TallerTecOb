using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace ObligatorioTTec.Models
{
    [Table ("Usuario")]
    public class Usuario
    {
        [Column("ID")]
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } 

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellido { get; set; }

        [Column("Correo")]
        [Unique]
        public string Correo { get; set; }


        [Column("Edad")]
        public int Edad { get; set; }

        [Column("Password")]
        public string Password { get; set; }

    }
}
