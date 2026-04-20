using AetherFire23.Commons.EntityFramework.DummyConsole.Context2;
using AetherFire23.Commons.EntityFramework.NpgsqlHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AertherFire23.EntityFramework.DummyModule2.Context2;

// Need a marker class for this... because no multiple inheritance. 
public class DummyNpgsql2 : NpgsqlInitializable<MyContext2>
{

}