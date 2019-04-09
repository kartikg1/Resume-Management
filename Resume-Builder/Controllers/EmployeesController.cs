using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Resume_Builder.Models;

namespace Resume_Builder.Controllers
{

    public class EmployeesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: api/Employees
        //public IQueryable<Employee> GetEmployees()
        //{
        //    return db.Employees;
        //}

        [Route("api/UserRole")]
        [HttpGet]
        public IHttpActionResult GetUserRole(string email)
        {
            
            var user = db.Users.Where(x => x.Email == email).Select(x =>new { x.Roles,x.EmployeeId });
            return Ok(user);
        }


        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }
            
            return Ok(employee);
        }
        

        [Route("api/EmployeeList")]
        [HttpGet]
        public IHttpActionResult GetEmployeeList()
        {
            var empList = from employee in db.Employees
                       join projectTeam in db.ProjectTeams on employee.EmployeeId equals projectTeam.EmployeeId
                       join project in db.Projects on projectTeam.ProjectId equals project.ProjectId

                       select new
                       {
                           employee.EmployeeId,
                           employee.EmployeeName,
                           project.ProjectTitle,
                           projectTeam.Role,
                           projectTeam.EmployeeTech
                       };
            if(empList==null)
            {
                return NotFound();
            }
            return Ok(empList);
        }

        [HttpGet]
        [Route("api/Employees/ProjectDetails/{EmployeeId}")]
        public IHttpActionResult ProjectDetails(int employeeId)
        {
            var result = from projectteam in db.ProjectTeams
                         join project in db.Projects on projectteam.ProjectId equals project.ProjectId
                         where projectteam.EmployeeId == employeeId

                         select new
                         {
                             projectteam.EmployeeTech,
                             projectteam.ProjectStatus,
                             projectteam.Role,
                             projectteam.EmployeeStartDate,
                             projectteam.EmployeeEndDate,
                             project.ProjectId,
                             project.ProjectTitle,
                             project.ProjectDescription,
                         };
            return Ok(result);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeId == id) > 0;
        }

        public int GetEmployeeId(string email)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int id = db.Employees.FirstOrDefault(x => x.EmployeeEmail == email).EmployeeId;
            return id;
        }
    }
}