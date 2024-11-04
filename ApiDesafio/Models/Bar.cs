using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDesafio.Models
{
    public class Bar
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O endereço não pode ter mais de 100 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "A descrição não pode ter mais de 300 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(0, 5.0, ErrorMessage = "A nota média deve ser entre 0 e 5.")]
        public double NotaMedia { get; set; }
    }
}