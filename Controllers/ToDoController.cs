using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TodoList.Data;
using TodoList.DTOs;
using TodoList.DTOs.ToDos;
using TodoList.Extensions;
using TodoList.Models;
using TodoList.ViewModels;

namespace TodoList.Controllers {
    [ApiController]
    [Route("v1/todos")]
    public class ToDoController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromServices] ToDoListDataContext context,
            [FromQuery] ToDoQueryDto dto) {
            try {

                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));


                var page = dto.Page;
                var pageSize = dto.PageSize;
                var isCompleted = dto.IsCompleted;
                var priority = dto.Priority;

                IQueryable<ToDo> contextQuery = context.ToDos.AsNoTracking();

                if (isCompleted == 0) {
                    contextQuery = contextQuery.Where(x => x.IsCompleted == false);
                }
                else if (isCompleted == 1) {
                    contextQuery = contextQuery.Where(x => x.IsCompleted == true);
                }

                if (priority > 0) {
                    contextQuery = contextQuery.Where(x => x.Priority == priority);
                }

                var count = await contextQuery.CountAsync();
                var toDos = await contextQuery
                    .Select(x => new ResponseToDoDto {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        IsCompleted = x.IsCompleted,
                        Priority = x.Priority,
                        CreatedAt = x.CreatedAt,
                        DueDate = x.DueDate,

                    })
                    .OrderBy(x => x.CreatedAt)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = new PagedResultDto<ResponseToDoDto> {
                    Itens = toDos,
                    Total = count,
                    Page = page,
                    PageSize = pageSize
                };

                return Ok(new ResultViewModel<PagedResultDto<ResponseToDoDto>>(result));
            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("01x01 Erro ao consultar o banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x02 Erro interno de servidor"));
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            [FromRoute] long id,
            [FromServices] ToDoListDataContext context) {
            try {
                var toDo = await context.ToDos
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new ResponseToDoDto {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        IsCompleted = x.IsCompleted,
                        Priority = x.Priority,
                        CreatedAt = x.CreatedAt,
                        DueDate = x.DueDate,
                    }).FirstOrDefaultAsync();

                if (toDo == null) {
                    return NotFound(new ResultViewModel<string>("01x03 Conteudo não encontrada"));
                }

                return Ok(new ResultViewModel<ResponseToDoDto>(toDo));
            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("01x04 Erro ao consultar o banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x05 Erro interno de servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] CreateToDoDto dto,
            [FromServices] ToDoListDataContext context) {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));

            try {
                var toDo = new ToDo {
                    Title = dto.Title,
                    Description = dto.Description,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                };

                await context.ToDos.AddAsync(toDo);
                await context.SaveChangesAsync();

                var response = new ResponseToDoDto {
                    Id = toDo.Id,
                    Title = toDo.Title,
                    Description = toDo.Description,
                    IsCompleted = toDo.IsCompleted,
                    Priority = toDo.Priority,
                    CreatedAt = toDo.CreatedAt,
                    DueDate = toDo.DueDate,
                };

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = toDo.Id },
                    new ResultViewModel<ResponseToDoDto>(response)
                );
            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("02x01 Erro ao inserir informação no banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x02 Erro interno de servidor"));
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Put(
            [FromRoute] long id,
            [FromBody] UpdateToDoDto dto,
            [FromServices] ToDoListDataContext context) {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));

            try {
                var toDo = await context.ToDos
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (toDo == null) {
                    return NotFound(new ResultViewModel<string>("03x01 Conteudo não encontrada para atualização"));
                }

                toDo.Title = dto.Title;
                toDo.Description = dto.Description;
                toDo.IsCompleted = dto.IsCompleted;
                toDo.Priority = dto.Priority;
                toDo.DueDate = dto.DueDate;

                context.ToDos.Update(toDo);
                await context.SaveChangesAsync();

                var response = new ResponseToDoDto {
                    Id = toDo.Id,
                    Title = toDo.Title,
                    Description = toDo.Description,
                    IsCompleted = toDo.IsCompleted,
                    Priority = toDo.Priority,
                    CreatedAt = toDo.CreatedAt,
                    DueDate = toDo.DueDate,
                };

                return Ok(new ResultViewModel<ResponseToDoDto>(response));
            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("03x02 Erro ao ataulizar informação no banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x03 Erro interno de servidor"));
            }
        }

        [HttpPatch("{id:long}")]
        public async Task<IActionResult> Patch(
            [FromRoute] long id,
            [FromBody] PatchToDoDto dto,
            [FromServices] ToDoListDataContext context) {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));

            try {
                var toDo = await context.ToDos
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (toDo == null) {
                    return NotFound(new ResultViewModel<string>("04x04 Conteudo não encontrada para atualização"));
                }

                if (dto.Title != null)
                    toDo.Title = dto.Title;

                if (dto.Description != null)
                    toDo.Description = dto.Description;

                if (dto.IsCompleted.HasValue)
                    toDo.IsCompleted = dto.IsCompleted.Value;

                if (dto.Priority.HasValue)
                    toDo.Priority = dto.Priority.Value;

                if (dto.DueDate.HasValue)
                    toDo.DueDate = dto.DueDate.Value;

                context.ToDos.Update(toDo);
                await context.SaveChangesAsync();

                var response = new ResponseToDoDto {
                    Id = toDo.Id,
                    Title = toDo.Title,
                    Description = toDo.Description,
                    IsCompleted = toDo.IsCompleted,
                    Priority = toDo.Priority,
                    CreatedAt = toDo.CreatedAt,
                    DueDate = toDo.DueDate,
                };
                return Ok(new ResultViewModel<ResponseToDoDto>(response));
            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("04x05 Erro ao ataulizar informação no banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x06 Erro interno de servidor"));
            }
        }

        [HttpDelete("{id:long}")]

        public async Task<IActionResult> Delete(
            [FromRoute] long id,

            [FromServices] ToDoListDataContext context) {

            try {
                var toDo = await context.ToDos
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (toDo == null) {
                    return NotFound(new ResultViewModel<string>("05x01 Conteudo não encontrada para exclusão"));
                }
                context.ToDos.Remove(toDo);
                await context.SaveChangesAsync();

                return Ok();

            }
            catch (DbException) {
                return BadRequest(new ResultViewModel<string>("05x02 Erro ao excluir informação no banco"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("05x03 Erro interno de servidor"));
            }
        }
    }
}