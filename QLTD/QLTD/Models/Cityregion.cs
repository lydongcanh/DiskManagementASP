using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
	public class CityRegion
	{
		public int Id { get; set; }
		public virtual City City
		{
			get;set;
		}
		public virtual Region Region
		{
			get;set;
		}
		public virtual Customer Customer
		{
			get;set;
		}
		public virtual ICollection<Store> Stores { get; set; }
		public virtual ICollection<Form> Forms { get; set; }
		public virtual ICollection<Candidate> Candidates { get; set; }

		//public virtual ICollection<Project> Projects { get; set; }

		public virtual ICollection<EProject> Projects { get; set; }
		public virtual ICollection<Vacancy> Vacancies { get; set; }
	}
}