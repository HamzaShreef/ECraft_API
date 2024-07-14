Use ECraftsDB;


Alter Procedure ViewCrafterProfile
@ProfileId int,
@ViewerId int
As
Begin
	If Exists ( Select 1 From ProfileViews Where ViewerId=@ViewerId And ProfileId=@ProfileId )
	Begin
		Update ProfileViews Set ViewsCount+=1,ViewDate=GETUTCDATE()
			Where ProfileId=@ProfileId And ViewerId=@ViewerId;
	End
	Else
	Begin
		Insert Into ProfileViews(ProfileId,ViewerId,ViewDate,ViewsCount)
		Values(@ProfileId,@ViewerId,GETUTCDATE(),1);

		Update Crafters Set ViewsCount+=1
			Where Id=@ProfileId;
	End
End



-- Get Profile


exec sp_executesql N'SELECT TOP(1) [c].[Id], [c].[About], [c].[AverageRating], [c].[ContactPhone], [c].[CraftId], [c].[ExperienceLevel], [c].[JoinDate], [c].[Latitude], [c].[LikesCount], [c].[Longitude], [c].[ProjectsCount], [c].[ReviewsCount], [c].[SkillsCount], [c].[Title], [c].[UserId], [c].[ViewsCount], [c].[WorkLocation], [a].[Id] AS [UserId], [a].[FirstName], [a].[LastName], [a].[UserName], [a].[ProfileImg] AS [Picture], [a].[MGender] AS [isMale], [c0].[Id], [c0].[CraftersCount], [c0].[CreationDate], [c0].[Description], [c0].[Icon], [c0].[Title], [l].[Id], [l].[CityName], [l].[CountryId], [l].[CraftersCount], [l].[LocalName], [l].[RegionId], [l].[TimeZone], [l].[UsersCount], [l0].[Id], [l0].[CountryCode], [l0].[CountryName], [l0].[LocalName], [l0].[TimeZone], [l1].[Id], [l1].[CountryId], [l1].[LocalName], [l1].[RegionName]
FROM [Crafters] AS [c]
INNER JOIN [AppUser] AS [a] ON [c].[UserId] = [a].[Id]
INNER JOIN [Crafts] AS [c0] ON [c].[CraftId] = [c0].[Id]
LEFT JOIN [LCities] AS [l] ON [a].[CityId] = [l].[Id]
LEFT JOIN [LCountries] AS [l0] ON [l].[CountryId] = [l0].[Id]
LEFT JOIN [LRegions] AS [l1] ON [l].[RegionId] = [l1].[Id]
WHERE [c].[UserId] = @__uid_0',N'@__uid_0 int',@__uid_0=1


-- Get Profile Custom Procedure


Alter Procedure GetCrafterProfile
@UID int
As
Begin
	SELECT TOP(1) 
	[c].[Id], [c].[About], [c].[AverageRating], [c].[ContactPhone], [c].[CraftId], [c].[ExperienceLevel], [c].[JoinDate],
	[c].[Latitude], [c].[LikesCount], [c].[Longitude], [c].[ProjectsCount], [c].[ReviewsCount], [c].[SkillsCount], [c].[Title],
	[c].[UserId], [c].[ViewsCount], [c].[WorkLocation], [a].[FirstName], [a].[LastName], [a].[UserName],
	[a].[ProfileImg] AS [Picture], [a].[MGender] AS [isMale], [c0].[CraftersCount], [c0].[CreationDate], [c0].[Description],
	[c0].[Icon], [c0].[Title], [l].[CityName], [l].[CountryId], [l].[CraftersCount], [l].[LocalName], [l].[RegionId], 
	[l].[TimeZone], [l].[UsersCount], [l0].[CountryCode], [l0].[CountryName], [l0].[LocalName], [l0].[TimeZone],
	[l1].[CountryId], [l1].[LocalName], [l1].[RegionName]
			FROM [Crafters] AS [c]
			INNER JOIN [AppUser] AS [a] ON [c].[UserId] = @UID And [c].[UserId] = [a].[Id]
			INNER JOIN [Crafts] AS [c0] ON [c].[CraftId] = [c0].[Id]
			LEFT JOIN [LCities] AS [l] ON [a].[CityId] = [l].[Id]
			LEFT JOIN [LCountries] AS [l0] ON [l].[CountryId] = [l0].[Id]
			LEFT JOIN [LRegions] AS [l1] ON [l].[RegionId] = [l1].[Id];
End

Exec GetCrafterProfile 1