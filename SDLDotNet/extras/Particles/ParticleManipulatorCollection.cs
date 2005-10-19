using System;
using System.Collections;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleManipulatorCollection.
	/// </summary>
	public class ParticleManipulatorCollection : CollectionBase, IParticleManipulator
	{
		public ParticleManipulatorCollection()
		{
		}
		public ParticleManipulatorCollection(IParticleManipulator manipulator)
		{
			Add(manipulator);
		}
		public ParticleManipulatorCollection(ParticleManipulatorCollection manipulators)
		{
			Add(manipulators);
		}


		public void Add(IParticleManipulator manipulator)
		{
			List.Add(manipulator);
		}

		public void Add(ParticleManipulatorCollection manipulators)
		{
			foreach(IParticleManipulator manipulator in manipulators)
			{
				List.Add(manipulator);
			}
		}
		public IParticleManipulator this[int index]
		{
			get
			{
				return (IParticleManipulator)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		#region IParticleManipulator Members

		public void Manipulate(ParticleCollection particles)
		{
			foreach(IParticleManipulator manipulator in List)
			{
				manipulator.Manipulate(particles);
			}
		}

		#endregion
	}
}
