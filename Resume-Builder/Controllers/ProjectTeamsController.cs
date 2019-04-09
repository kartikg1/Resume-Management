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
    
    public class ProjectTeamsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProjectTeams
        public IQueryable<ProjectTeam> GetProjectTeams()
        {
            return db.ProjectTeams;
        }

        //GET: api/ProjectTeams/5
        [ResponseType(typeof(ProjectTeam))]
        public IHttpActionResult GetProjectTeam(int id)
        {
            var team = from employee in db.Employees
                       join projectTeam in db.ProjectTeams on employee.EmployeeId equals projectTeam.EmployeeId
                       join project in db.Projects on projectTeam.ProjectId equals project.ProjectId
                       where projectTeam.ProjectId == id && projectTeam.ProjectStatus==false
                       select new
                       {
                           projectTeam.EmployeeId,
                           project.ProjectDescription,                           
                           employee.EmployeeEmail,
                           employee.EmployeeName,
                           projectTeam.Role,
                           projectTeam.EmployeeTech,
                           project.ProjectId
                       };          
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        // PUT: api/ProjectTeams/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectTeam(int id, ProjectTeam projectTeam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != projectTeam.TeamId)
            {
                return BadRequest();
            }
            db.Entry(projectTeam).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTeamExists(id))
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

        [HttpPut]
        [Route("api/ProjectTeamsRelease/{projectId}/{employeeId}")]
        public IHttpActionResult ProjectTeamsRelease(int projectId,int employeeId)
        {
            ProjectTeam projectTeam = db.ProjectTeams.FirstOrDefault(x => x.EmployeeId == employeeId && x.ProjectId==projectId && x.ProjectStatus == false);
            projectTeam.ProjectStatus = true;
            projectTeam.EmployeeEndDate = DateTime.Now.Date;
            db.SaveChanges();
            return Ok();
        }

        // POST: api/ProjectTeams
       // [Route("api/ProjectTeams/AssignProject")]
        [ResponseType(typeof(ProjectTeam))]
        public IHttpActionResult PostProjectTeam(ProjectTeam projectTeam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // check if EmployeeEmailId exists
            var employeeEmails = db.Employees.Select(x => x.EmployeeEmail);
            if (!employeeEmails.Contains(projectTeam.EmployeeEmail))
            {
                return BadRequest("Invalid Email");                
            }
            projectTeam.EmployeeId = db.Employees.SingleOrDefault(x => x.EmployeeEmail == projectTeam.EmployeeEmail).EmployeeId;
            // check if the Employee is already added to the project(redundant/multiple entries)
            var ActiveEmployeeIds = db.ProjectTeams.AsEnumerable()
                 .Where(x => x.ProjectId == projectTeam.ProjectId).Select(x => x.EmployeeId)
                 .ToList();
            if (ActiveEmployeeIds.Contains(projectTeam.EmployeeId))
            {
                return BadRequest("Employee Already exists in the project");
            }
            // check if the employee is assigned to a new technology
            var employeetech = db.ProjectTeams.AsEnumerable()
                .Where(x => x.EmployeeId == projectTeam.EmployeeId).Select(x => x.EmployeeTech)
                .ToList();
            if (!employeetech.Contains(projectTeam.EmployeeTech))
            {
                db.ProjectTeams.Add(projectTeam);
                db.SaveChanges();
                return Content(HttpStatusCode.BadRequest, "Employee Assigned to new Technology");
            }
            db.ProjectTeams.Add(projectTeam);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = projectTeam.TeamId }, projectTeam);
        }

        [HttpDelete]
        [Route("api/ProjectTeamsDelete/{projectId}/{employeeId}")]
        public IHttpActionResult DeleteProjectTeam(int projectId,int employeeId)
        {
            ProjectTeam projectTeam = db.ProjectTeams.FirstOrDefault(x => x.ProjectId == projectId && x.EmployeeId == employeeId && x.ProjectStatus==false);
            db.ProjectTeams.Remove(projectTeam);
            db.SaveChanges();
            return Ok();
        }

        [Route("api/EmployeeTechCount")]
        [HttpGet]
        public IHttpActionResult GetEmployeeTechCount()
        {
            var EmployeeTechCount = db.ProjectTeams.GroupBy(x => x.EmployeeTech)
                         .Select(group => new
                         {
                             Tech = group.Key,
                             Count = group.Count()
                         });
            return Ok(EmployeeTechCount);
        }

        [Route("api/EmployeeRoleCount")]
        [HttpGet]
        public IHttpActionResult GetEmployeeRoleCount()
        {
            var EmployeeRoleCount = db.ProjectTeams.GroupBy(x => x.Role)
                         .Select(group => new
                         {
                             Tech = group.Key,
                             Count = group.Count()
                         });
            return Ok(EmployeeRoleCount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectTeamExists(int id)
        {
            return db.ProjectTeams.Count(e => e.TeamId == id) > 0;
        }
    }
}


