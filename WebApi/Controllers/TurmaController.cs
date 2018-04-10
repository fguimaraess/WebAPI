using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    public class TurmaController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<Turma> Get()
        {
            using (elevaEntities DAL = new elevaEntities())
            {
                IEnumerable<Turma> Turma = DAL.Turma.Include("Escola").ToList();
                return Turma;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public Turma Get(int id)
        {
            using (var DAL = new elevaEntities())
            {
                Turma Model = DAL.Turma.Where(x => x.ID == id).FirstOrDefault();

                return Model;
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*"), HttpPost]
        public IHttpActionResult Create([FromBody] Turma turma)
        {
            if (turma == null)
            {
                return BadRequest();
            }
            using (var DAL = new elevaEntities())
            {
                if (ModelState.IsValid)
                {
                    var Escola = DAL.Escola.Where(x=>x.ID == turma.IDEscola).FirstOrDefault();
                    if (Escola == null)
                        return BadRequest();
                    DAL.Entry(turma).State = EntityState.Added;

                    DAL.SaveChanges();
                }
            }
            return Ok();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*"), HttpPut]
        public IHttpActionResult Update(long id, [FromBody] Turma turma)
        {
            if (turma == null || turma.ID != id)
            {
                return BadRequest();
            }

            using (var DAL = new elevaEntities())
            {
                var Model = DAL.Turma.Where(x => x.ID == id).FirstOrDefault();

                if (Model == null)
                {
                    return NotFound();
                }

                var Escola = DAL.Escola.Where(x => x.ID == turma.IDEscola).FirstOrDefault();
                if (Escola == null)
                    return BadRequest();

                Model.Nome = turma.Nome;
                Model.IDEscola = turma.IDEscola;

                DAL.Entry(Model).State = EntityState.Modified;

                DAL.SaveChanges();
            }
            return Ok();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*"), HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            using (var DAL = new elevaEntities())
            {
                var Model = DAL.Turma.Where(x => x.ID == id).FirstOrDefault();
                if (Model == null)
                {
                    return NotFound();
                }

                DAL.Entry(Model).State = EntityState.Deleted;
                DAL.SaveChanges();
                return Ok();
            }
        }
    }
}
