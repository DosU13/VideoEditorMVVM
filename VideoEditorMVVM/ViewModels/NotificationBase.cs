using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorMVVM.ViewModels
{
	/// <summary>
	/// Forked from https://github.com/johnshew/Minimal-UWP-MVVM-CRUD/blob/master/Simple%20MVVM%20UWP%20with%20CRUD/ViewModels/ViewModelHelpers.cs
	/// </summary>
	public class NotificationBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// SetField (Name, value); // where there is a data member
		protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string property = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			NotifyPropertyChanged(property);
			return true;
		}

		// SetField(()=> somewhere.Name = value; somewhere.Name, value) // Advanced case where you rely on another property
		protected void SetProperty<T>(T currentValue, T newValue, Action doSet, [CallerMemberName] string property = null)
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue)) return;
			doSet.Invoke();
			NotifyPropertyChanged(property);
		}
	}

	public class NotificationBase<T> : NotificationBase where T : class
	{
		protected readonly T This;

		public static implicit operator T(NotificationBase<T> thing) { return thing.This; }

		protected NotificationBase(T thing)
		{
			This = thing;
		}
	}
}
