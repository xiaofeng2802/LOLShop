using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DataSeedWork.LuckyWheel
{
    public class ProportionValue<T>
    {
        public double Proportion { get; set; }
        public T Value { get; set; }
    }

    public static class ProportionValue
    {
        public static ProportionValue<T> Create<T>(double proportion, T value)
        {
            return new ProportionValue<T> { Proportion = proportion, Value = value };
        }

        static Random random = new Random();

        public static T ChooseByRandom<T>(
            this IEnumerable<ProportionValue<T>> collection, out int resultForWheel)
        {
            int index = 1;
            var rnd = random.NextDouble();
            foreach (var item in collection)
            {
                if (rnd < item.Proportion)
                {
                    resultForWheel = index;
                    return item.Value;
                }

                rnd -= item.Proportion;
                index++;
            }
            throw new InvalidOperationException(
                "The proportions in the collection do not add up to 1.");
        }
    }
}
