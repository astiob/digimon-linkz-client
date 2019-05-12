using System;

public class LaboratoryAccessor : ClassSingleton<LaboratoryAccessor>
{
	public CMD_Laboratory laboratory { get; set; }
}
