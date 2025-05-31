using AutoMapper;
using EnrollmentManagement.Data;
using EnrollmentManagement.Dtos;
using EnrollmentManagement.Models;
using EnrollmentManagement.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly AppDbContext _context; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EnrollmentController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        //Métodos crud básicos
        /// <summary>
        /// Crea una nueva matrícula.
        /// </summary>
        /// <param name="dto">DTO con ID del estudiante, ID del curso y fecha de matrícula.</param>
        /// <returns>ID de la matrícula creada si es exitoso.</returns>
        /// <response code="200">Matrícula creada correctamente.</response>
        /// <response code="400">Error de validación: estudiante/curso no existe, fecha inválida o matrícula duplicada.</response>
        [HttpPost]
        public async Task<IActionResult> CreateEnrollment(CreateEnrollmentDto dto)
        {
            try
            {
                _logger.LogInformation("Creando matrícula para StudentId {StudentId}, CourseId {CourseId}", dto.StudentId, dto.CourseId);

                bool studentExists = await _unitOfWork.Students.ExistsAsync(dto.StudentId);
                if (!studentExists)
                {
                    _logger.LogWarning("Estudiante no existe: {StudentId}", dto.StudentId);
                    return BadRequest("Estudiante no existe");
                }

                bool courseExists = await _unitOfWork.Courses.ExistsAsync(dto.CourseId);
                if (!courseExists)
                {
                    _logger.LogWarning("Curso no existe: {CourseId}", dto.CourseId);
                    return BadRequest("Curso no existe");
                }

                if (dto.EnrollmentDate > DateTime.Now)
                {
                    _logger.LogWarning("Fecha de matrícula futura: {Date}", dto.EnrollmentDate);
                    return BadRequest("La fecha de matrícula no puede ser futura");
                }

                bool duplicate = await _unitOfWork.Enrollments
                    .ExistsAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);

                if (duplicate)
                {
                    _logger.LogWarning("Intento duplicado de matrícula para StudentId {StudentId}, CourseId {CourseId}", dto.StudentId, dto.CourseId);
                    return BadRequest("El estudiante ya está matriculado en ese curso");
                }

                var enrollment = _mapper.Map<Enrollment>(dto);
                enrollment.Status = "Activa";

                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Matrícula creada con Id {EnrollmentId}", enrollment.Id);

                return Ok(enrollment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando matrícula para StudentId {StudentId}, CourseId {CourseId}", dto.StudentId, dto.CourseId);
                return StatusCode(500, "Error interno del servidor");
            }
        }
        /// <summary>
        /// Actualiza el estado de una matrícula existente.
        /// </summary>
        /// <param name="id">ID de la matrícula.</param>
        /// <param name="dto">Nuevo estado a asignar: "Activa", "Finalizada", o "Cancelada".</param>
        /// <returns>Confirmación de cambio de estado.</returns>
        /// <response code="200">Estado actualizado exitosamente.</response>
        /// <response code="400">No se permite cancelar una matrícula ya finalizada.</response>
        /// <response code="404">Matrícula no encontrada.</response>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateEnrollmentStatusDto dto)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);

            if (enrollment == null)
                return NotFound("Matrícula no encontrada");

            if (enrollment.Status == "Finalizada" && dto.Status == "Cancelada")
                return BadRequest("No se puede cancelar una matrícula ya finalizada");

            enrollment.Status = dto.Status;
            await _unitOfWork.CompleteAsync();

            return Ok("Estado actualizado");
        }
        /// <summary>
        /// Elimina una matrícula solo si su estado es "Cancelada".
        /// </summary>
        /// <param name="id">ID de la matrícula a eliminar.</param>
        /// <returns>Confirmación de eliminación.</returns>
        /// <response code="200">Matrícula eliminada correctamente.</response>
        /// <response code="400">Solo se permiten eliminar matrículas con estado "Cancelada".</response>
        /// <response code="404">Matrícula no encontrada.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                return NotFound("Matrícula no encontrada");

            if (enrollment.Status != "Cancelada")
                return BadRequest("Solo se pueden eliminar matrículas canceladas");

            _unitOfWork.Enrollments.Delete(enrollment);
            await _unitOfWork.CompleteAsync();

            return Ok("Matrícula eliminada");
        }

    }

}
