using System;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Helpers
{
	public class FormatDate
	{
		public string Date(DateTime dateTime) {

            var prevDate = dateTime; 
            var today = DateTime.Now;

            var SubDate = today.Subtract(prevDate);


            if ((int)SubDate.TotalSeconds < 59)
                return $" {(int)SubDate.TotalSeconds}s";
            if ((int)SubDate.TotalMinutes < 59)
                return $" {(int)SubDate.TotalMinutes}m";
            if ((int)SubDate.TotalHours < 23)
                return $" {(int)SubDate.TotalHours}h";
            if ((int)SubDate.TotalDays < 30)
                return $" {(int)SubDate.TotalDays}d";
            if ((int)SubDate.TotalDays < 350)
                return prevDate.ToString("d MMMM");



            return prevDate.ToString("d MMMM yyyy");
        }
	}
}

