using E_Learning.DAL;
using E_Learning.BL;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


#region Identity

//Mainly specify the context and the type of the user that the UserManger will use
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;

    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ELearningContext>()
    .AddDefaultTokenProviders();


#endregion

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "row"; // For Authentication
    options.DefaultChallengeScheme = "row"; //To Handle Challenge
})
    .AddJwtBearer("row", options =>
    {
        //Use this key when validating requests
        var keyString = builder.Configuration.GetValue<string>("SecretKey");
        var keyInBytes = Encoding.ASCII.GetBytes(keyString!);
        var key = new SymmetricSecurityKey(keyInBytes);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
#region Service

//database
builder.Services.AddDbContext<ELearningContext>();

//repos
builder.Services.AddScoped<IUserrepository, Userrepository>();
builder.Services.AddScoped<IUserAnswerrepository, UserAnswerrepository>();
builder.Services.AddScoped<IClassrepository, Classrepository>();
builder.Services.AddScoped<ILecturerepository, Lecturerepository>();
builder.Services.AddScoped<IAssigmentrepository, Assigmentrepository>();
builder.Services.AddScoped<IUserLecturerepository, UserLecturerepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 


//mangers 

builder.Services.AddScoped<IUserManger, UserManger>();
builder.Services.AddScoped<IClassManger, ClassManger>();
builder.Services.AddScoped< ILectureManger, LectureManger>();
builder.Services.AddScoped<IAssighmentManger, AssighmentManger>();


#endregion
var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
    
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
app.UseCors("AllowAll");


app.MapControllers();

            app.Run();
        
    
