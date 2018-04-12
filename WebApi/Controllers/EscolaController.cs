using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class EscolaController : ApiController
    {
        public IEnumerable<Escola> Get()
        {
            using (elevaEntities DAL = new elevaEntities())
            {
                IEnumerable<Escola> Escola = DAL.Escola.Include("Turma").ToList();
                return Escola;
            }
        }

        public Escola Get(int id)
        {
            using(var DAL = new elevaEntities())
            {
                Escola Model = DAL.Escola.Where(x => x.ID == id).Include("Turma").FirstOrDefault();

                return Model;
            }
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] Escola escola)
        {
            //var Model = JsonConvert.DeserializeObject<Escola>(escola);
            if (escola == null)
            {
                return BadRequest();
            }
            using (var DAL = new elevaEntities())
            {
                if (ModelState.IsValid)
                {
                    DAL.Entry(escola).State = EntityState.Added;

                    DAL.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Update(long id, [FromBody] Escola escola)
        {
            if (escola == null || escola.ID != id)
            {
                return BadRequest();
            }

            using (var DAL = new elevaEntities())
            {
                var Model = DAL.Escola.Where(x => x.ID == id).FirstOrDefault();

                if (Model == null)
                {
                    return NotFound();
                }

                Model.Nome = escola.Nome;
                Model.Endereco = escola.Endereco;

                DAL.Entry(Model).State = EntityState.Modified;

                DAL.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            using (var DAL = new elevaEntities())
            {
                var Model = DAL.Escola.Where(x => x.ID == id).Include("Turma").FirstOrDefault();
                if(Model == null)
                {
                    return NotFound();
                }

                var Turmas = Model.Turma.ToList();
                foreach (var item in Turmas)
                {
                    DAL.Entry(item).State = EntityState.Deleted;
                }

                DAL.Entry(Model).State = EntityState.Deleted;
                DAL.SaveChanges();
                return Ok();
            }
        }

    }
}
