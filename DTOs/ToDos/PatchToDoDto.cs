using System.ComponentModel.DataAnnotations;

namespace TodoList.DTOs.ToDos {
    public class PatchToDoDto {

        [MaxLength(200, ErrorMessage = "O título não pode exceder 200 caracteres")]
        [MinLength(3, ErrorMessage = "O título deve ter no mínimo 3 caracteres")]
        public string? Title { get; set; }
        public string? Description { get; set; }

        public bool? IsCompleted { get; set; }

        [Range(1, 5, ErrorMessage = "A prioridade deve estar entre 1 e 5")]
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
