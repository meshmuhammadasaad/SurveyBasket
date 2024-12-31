using Mapster;
using SurveyBasket.Api.Contracts.Questions;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, scr => scr.Answers.Select(ans => new Answer { Content = ans }));
    }
}
