create procedure [dbo].[GetLastPreRent]
	@ClientUserId int
as
begin
	select top 1 * from Rents r 
	where r.ClientUserId = @ClientUserId and r.IsEnabled = 1 and r.StatusId = 2
	order by r.Date desc
end

create procedure [dbo].[GetLastPreRentItems]
	@PreRentId uniqueidentifier
as
begin
	select rI.Id, m.Id as MovieId, rI.RentDuration, m.Title, m.FirebasePosterId, m.Price
	from RentItems rI 
	join Movies m on rI.MovieId = m.Id
	where rI.RentId = @PreRentId;
end