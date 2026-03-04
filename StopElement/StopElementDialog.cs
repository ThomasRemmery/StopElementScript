namespace StopElement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	internal class StopElementDialog : Dialog
	{
		private readonly Label pickElementLabel = new Label("Pick an element:");
		private readonly DropDown<IDmsElement> activeElementDropDown = new DropDown<IDmsElement>();
		private readonly Button stopElementButton = new Button("Stop element");

		public StopElementDialog(IEngine engine) : base(engine)
		{
			Title = "Stop Element";

			SetDropDownOptions();
			BuildUi();
			HandleEvents();
		}

		private void SetDropDownOptions()
		{
			var dms = Engine.GetDms();
			var activeElements = dms.GetElements().Where(e => e.State == ElementState.Active).ToList();
			var options = activeElements.Select(x => new Option<IDmsElement>(x.Name, x));
			activeElementDropDown.SetOptions(options);
		}

		private void BuildUi()
		{
			AddWidget(pickElementLabel, 0, 0);
			AddWidget(activeElementDropDown, 0, 1);
			AddWidget(stopElementButton, 1, 0, 1, 2, HorizontalAlignment.Right);
		}

		private void HandleEvents()
		{
			stopElementButton.Pressed += (s, e) =>
			{
				if (activeElementDropDown.Selected == null)
					return;

				activeElementDropDown.Selected.Stop();
				Engine.ExitSuccess("Stopped element: " + activeElementDropDown.Selected.Name);
			};
		}
	}
}
