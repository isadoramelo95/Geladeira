using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace GeladeiraAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeladeiraController : ControllerBase
    {
        IGeladeiraService<Item> _services;
        public GeladeiraController(IGeladeiraService<Item> services)
        {
            _services = services;
        }

        [HttpHead]
        public async Task<IActionResult> CheckStatusGeladeira()
        {
            List<Item> Items = await _services.ListaDeItens();
            Response.Headers.Append("Total", Items.Count.ToString());
            return Ok();
        }

        [HttpGet("ListaItens")]
        public async Task<ActionResult<IEnumerable<Item>>> GetListaItensAsync()
        {
            try
            {
                return Ok(await _services.ListaDeItens());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _services.GetItemById(id);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("AdicionarItem")]
        public async Task<IActionResult> PostItem([FromBody] Item item)
        {
            try
            {
                await _services.AddNaGeladeira(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddLista")]
        public async Task<IActionResult> PostList([FromBody] List<Item> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest("A lista de itens não pode ser nula ou vazia.");
            }
            try
            {
                return Ok(await _services.AdicionarListaItensGeladeira(items));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AtualizarItem")]
        public async Task<IActionResult> PutById([FromBody] Item item)
        {
            try
            {
                await _services.EditarItemNaGeladeira(item);
                return Ok("Item alterado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoverPorId")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var item = _services.GetItemById(id);

                if (item == null)
                    return NotFound();

                await _services.RemoverItembyId(id);

                return Ok("Item retirado da geladeira!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("EsvaziarGeladeiraPorContainer")]
        public async Task<IActionResult> EsvaziarPorContainer(int numContainer)
        {
            try
            {
                return Ok(await _services.EsvaziarPorContainer(numContainer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}