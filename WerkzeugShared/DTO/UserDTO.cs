using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugShared.DTO
{
    public class UserDTO
    {
        [Key]
        public string Benutzername { get; set; }
        public string Passwort { get; set; }
    }
}
