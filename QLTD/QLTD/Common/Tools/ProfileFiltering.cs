using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ehr.Bussiness;
using Ehr.Data;
using Ehr.Models;
using Ehr.Common.Constraint;

namespace Ehr.Common.Tools
{
	public class ProfileFiltering
	{
        public ProfileFiltering()
        {
        }
		//public List<Candidate> Cast2List ( List<List<Candidate>> list )
		//{
		//	List<Candidate> newList = new List<Candidate> ( );
		//	foreach(List<Candidate> l in list)
		//	{
		//		newList.AddRange ( l );
		//	}
		//	return newList;
		//}
		///// <summary>
		///// Lấy ra danh sách của các hồ sơ bị trùng theo bộ
		///// </summary>
		///// <param name="projectID"></param>
		///// <returns></returns>
		//public List<List<Candidate>> GetDuplication ( int projectID )
		//{
		//	 EhrDbContext db = new EhrDbContext();
		//	//Kiểm tra trùng IP, trùng tên, trùng số điện thoại,trạng thái hồ sơ chưa duyệt => Trùng hồ sơ
		//	var duplicates = db.Candidates.Where(c=>c.Form.Project.Id.Equals(projectID)&&c.Approval!=ApprovalStatus.DELETED)
		//					.GroupBy(c => new { c.IP, c.FullName,c.PhoneNumber })
		//					.Select(g => new { Qty = g.Count(), First = g.OrderBy(c => c.Id).ToList() } )
		//					.Where(t=>t.Qty>1&&t.Qty<4)
		//					.Select(p => new
		//						{
		//							id = p.First
		//						}).ToList();
		//	//List<int> dup= duplicates.Cast<int>().ToList();
		//	List<List<Candidate>> list = new List<List<Candidate>> ( );
		//	foreach(var cad in duplicates)
		//	{
		//		list.Add ( cad.id );
		//	}
		//	return list;
		//}
		
		///// <summary>
		///// Lấy ra danh sách của các hồ sơ bị trùng theo vị trí công việc
		///// </summary>
		///// <param name="projectID"></param>
		///// <param name="vacancy"></param>
		///// <returns></returns>
		//public List<List<Candidate>> GetDuplication ( int projectID, int vacancy )
		//{
		//	 EhrDbContext db = new EhrDbContext();
		//	//Kiểm tra trùng IP, trùng tên, trùng số điện thoại,trạng thái hồ sơ chưa duyệt => Trùng hồ sơ
		//	var duplicates = db.Candidates.Where(c=>c.Form.Project.Id.Equals(projectID)&&c.Position.Id==vacancy&&c.Approval!=ApprovalStatus.DELETED)
		//					.GroupBy(c => new { c.IP, c.FullName,c.PhoneNumber })
		//					.Select(g => new { Qty = g.Count(), First = g.OrderBy(c => c.Id).ToList() } )
		//					.Where(t=>t.Qty>1&&t.Qty<4)
		//					.Select(p => new
		//						{
		//							id = p.First
		//						}).ToList();
		//	//List<int> dup= duplicates.Cast<int>().ToList();
		//	List<List<Candidate>> list = new List<List<Candidate>> ( );
		//	foreach(var cad in duplicates)
		//	{
		//		list.Add ( cad.id );
		//	}
		//	return list;
		//}
		///// <summary>
		///// Lấy ra danh sách của các hồ sơ dạng spam theo vị trí công việc
		///// </summary>
		///// <param name="projectID"></param>
		///// <param name="vacancy"></param>
		///// <returns></returns>
		//public List<List<Candidate>> GetSpam ( int projectID, int vacancy  )
		//{
		//	 EhrDbContext db = new EhrDbContext();
		//	//Kiểm tra trùng IP, trùng email, trùng giới tính, trùng số điện thoại => spam
		//	var duplicates = db.Candidates.Where(c=>c.Form.Project.Id.Equals(projectID)&&c.Position.Id==vacancy&&c.Approval!=ApprovalStatus.DELETED)
		//					.GroupBy(c => new { c.IP, c.FullName,c.PhoneNumber })
		//					.Select(g => new { Qty = g.Count(), First = g.OrderBy(c => c.Id).ToList() } )
		//					.Where(t=>t.Qty>2)
		//					.Select(p => new
		//						{
		//							id = p.First
		//						}).ToList();
		//	//List<int> dup= duplicates.Cast<int>().ToList();
		//	List<List<Candidate>> list = new List<List<Candidate>> ( );
		//	foreach(var cad in duplicates)
		//	{
		//		list.Add ( cad.id );
		//	}
		//	return list;
		//}
		///// <summary>
		///// Lấy ra danh sách của các hồ sơ dạng spam
		///// </summary>
		///// <param name="projectID"></param>
		///// <returns></returns>
		//public List<List<Candidate>> GetSpam ( int projectID )
		//{
		//	 EhrDbContext db = new EhrDbContext();
		//	//Kiểm tra trùng IP, trùng email, trùng giới tính, trùng số điện thoại => spam
		//	var duplicates = db.Candidates.Where(c=>c.Form.Project.Id.Equals(projectID)&&c.Approval!=ApprovalStatus.DELETED)
		//					.GroupBy(c => new { c.IP, c.FullName,c.PhoneNumber })
		//					.Select(g => new { Qty = g.Count(), First = g.OrderBy(c => c.Id).ToList() } )
		//					.Where(t=>t.Qty>1)
		//					.Select(p => new
		//						{
		//							id = p.First
		//						}).ToList();
		//	//List<int> dup= duplicates.Cast<int>().ToList();
		//	List<List<Candidate>> list = new List<List<Candidate>> ( );
		//	foreach(var cad in duplicates)
		//	{
		//		list.Add ( cad.id );
		//	}
		//	return list;
		//}
	}
}