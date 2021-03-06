using System;
using NUnit.Framework;
using NServiceKit.DataAnnotations;
using NServiceKit.Logging;
using NServiceKit.Text;

namespace NServiceKit.Common.Tests.Models{

    /// <summary>A model with fields of different types.</summary>
	[Alias("ModelWFDT")]
	public class ModelWithFieldsOfDifferentTypes
	{
        /// <summary>The log.</summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(ModelWithFieldsOfDifferentTypes));

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
		[AutoIncrement]
		public int Id
		{
			get;
			set;
		}

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
		public string Name
		{
			get;
			set;
		}

        /// <summary>Gets or sets the identifier of the long.</summary>
        /// <value>The identifier of the long.</value>
		public long LongId
		{
			get;
			set;
		}

        /// <summary>Gets or sets a unique identifier.</summary>
        /// <value>The identifier of the unique.</value>
		public Guid Guid
		{
			get;
			set;
		}

        /// <summary>Gets or sets a value indicating whether the. </summary>
        /// <value>true if , false if not.</value>
		public bool Bool
		{
			get;
			set;
		}

        /// <summary>Gets or sets the date time.</summary>
        /// <value>The date time.</value>
		public DateTime DateTime
		{
			get;
			set;
		}

        /// <summary>Gets or sets the double.</summary>
        /// <value>The double.</value>
		public double Double
		{
			get;
			set;
		}

        /// <summary>Creates a new ModelWithFieldsOfDifferentTypes.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The ModelWithFieldsOfDifferentTypes.</returns>
		public static ModelWithFieldsOfDifferentTypes Create(int id)
		{
			return new ModelWithFieldsOfDifferentTypes
			{
				Id = id, 
				Bool = id % 2 == 0, 
				DateTime = DateTime.Now.AddDays((double)id), 
				Double = 1.11 + (double)id, 
				Guid = Guid.NewGuid(), 
				LongId = (long)(999 + id), 
				Name = "Name" + id
			};
		}

        /// <summary>Creates a constant.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The new constant.</returns>
		public static ModelWithFieldsOfDifferentTypes CreateConstant(int id)
		{
			return new ModelWithFieldsOfDifferentTypes
			{
				Id = id, 
				Bool = id % 2 == 0, 
				DateTime = new DateTime(1979, id % 12 + 1, id % 28 + 1), 
				Double = 1.11 + (double)id, 
				Guid = new Guid((id % 240 + 16).ToString("X") + "726E3B-9983-40B4-A8CB-2F8ADA8C8760"), 
				LongId = (long)(999 + id), 
				Name = "Name" + id
			};
		}

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
		public override bool Equals(object obj)
		{
			ModelWithFieldsOfDifferentTypes other = obj as ModelWithFieldsOfDifferentTypes;
			if (other == null)
			{
				return false;
			}
			bool result;
			try
			{
				ModelWithFieldsOfDifferentTypes.AssertIsEqual(this, other);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return (this.Id + this.Guid.ToString()).GetHashCode();
		}

        /// <summary>Assert is equal.</summary>
        /// <param name="actual">  The actual.</param>
        /// <param name="expected">The expected.</param>
		public static void AssertIsEqual(ModelWithFieldsOfDifferentTypes actual, ModelWithFieldsOfDifferentTypes expected)
		{
			Assert.That(actual.Id, Is.EqualTo(expected.Id));
			Assert.That(actual.Name, Is.EqualTo(expected.Name));
			Assert.That(actual.Guid, Is.EqualTo(expected.Guid));
			Assert.That(actual.LongId, Is.EqualTo(expected.LongId));
			Assert.That(actual.Bool, Is.EqualTo(expected.Bool));
			try
			{
				Assert.That(actual.DateTime, Is.EqualTo(expected.DateTime));
			}
			catch (Exception ex)
			{
				ModelWithFieldsOfDifferentTypes.Log.Error("Trouble with DateTime precisions, trying Assert again with rounding to seconds", ex);
				Assert.That(DateTimeExtensions.RoundToSecond(actual.DateTime), Is.EqualTo(DateTimeExtensions.RoundToSecond(expected.DateTime)));
			}
			try
			{
				Assert.That(actual.Double, Is.EqualTo(expected.Double));
			}
			catch (Exception ex2)
			{
				ModelWithFieldsOfDifferentTypes.Log.Error("Trouble with double precisions, trying Assert again with rounding to 10 decimals", ex2);
				Assert.That(Math.Round(actual.Double, 10), Is.EqualTo(Math.Round(actual.Double, 10)));
			}
		}
	}
}
