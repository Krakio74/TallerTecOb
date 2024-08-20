
/* Unmerged change from project 'ObligatorioTTec (net8.0-windows10.0.19041.0)'
Before:
using System;
After:
using SQLite;
using System;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-android)'
Before:
using System;
After:
using SQLite;
using System;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-maccatalyst)'
Before:
using System;
After:
using SQLite;
using System;
*/
//using System.ComponentModel.DataAnnotations.Schema;
using
/* Unmerged change from project 'ObligatorioTTec (net8.0-windows10.0.19041.0)'
Before:
using SQLite;
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-android)'
Before:
using SQLite;
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-maccatalyst)'
Before:
using SQLite;
using System.Threading.Tasks;
After:
using System.Threading.Tasks;
*/
SQLite;

namespace ObligatorioTTec.Models
{
    [Table("Usuario")]
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

        [Column("Pass")]
        public string Pass { get; set; }

    }
}
