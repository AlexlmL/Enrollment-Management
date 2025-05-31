using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace EnrollmentManagement.Dtos
{
    public class UpdateEnrollmentStatusDto
    {
        //Required to identify the enrollment being updated
        [Required]
        [StringLength(20)]
        [SwaggerSchema(Description = "Nuevo estado de la matrícula. Ej: Activa, Cancelada, Finalizada")]
        public string Status { get; set; }
    }
}
