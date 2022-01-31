using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoEditorMVVM.Models;

namespace VideoEditorMVVM.ViewModels
{
    public class TaskViewModel : NotificationBase<Task>
	{
		private Task _task;

		public TaskViewModel()
		{
			_task = new Task();
		}

		public string Name
		{
			get { return This.Name; }
			set { SetProperty(This.Name, value, () => This.Name = value); }
		}
	}
}
