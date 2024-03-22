using Bogus;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Base;
public class BaseFixture
{
    protected Faker Faker {  get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }
}
