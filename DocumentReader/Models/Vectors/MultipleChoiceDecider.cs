using DocumentReader.Repositories;

namespace CarelessApi.Models.Vectors
{
    public class MultipleChoiceDecider<T>
    {
        private List<Choice<T>> choices;
        private VectorCollection<EmbeddedObject<T>>? embeddedObjects;

        private T outOfRangeOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceDecider{T}"/> class.
        /// Creates a new multiple choice decider
        /// </summary>
        /// <param name="outOfRangeOption">The out of range option is the value that will be returned when no option matches</param>
        public MultipleChoiceDecider(T outOfRangeOption)
        {
            choices = new List<Choice<T>>();
            this.outOfRangeOption = outOfRangeOption;
        }

        public Choice<T> AddChoice(Choice<T> choice)
        {
            choices.Add(choice);
            return choice; // for nice method chaining syntax
        }

        public Choice<T> AddChoice(T value, float? minimumSimilarity = null)
        {
            Choice<T> choice = new Choice<T>(value, minimumSimilarity);
            choices.Add(choice);
            return choice; // for nice method chaining syntax
        }

        /// <summary>
        /// Will organize all the data and embedd everything so that a decision can be made. Has to be called before Decide can be called
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task EmbeddAsync(IStoredEmbeddingRepository storedEmbeddingRepository)
        {
            List<EmbeddedObject<T>> result = new List<EmbeddedObject<T>>();

            foreach (Choice<T> choice in choices)
            {
                List<EmbeddedObject<T>> objects = await choice.GetEmbeddedObjects(storedEmbeddingRepository);

                foreach (EmbeddedObject<T> embeddedObject in objects)
                    result.Add(embeddedObject);
            }

            embeddedObjects = new VectorCollection<EmbeddedObject<T>>();
            embeddedObjects.AddRange(result);
        }
    }
}
