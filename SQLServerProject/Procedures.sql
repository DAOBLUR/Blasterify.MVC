create procedure [dbo].[GetLastPreRent]
	@ClientUserId int
as
begin
	select top 1 * from PreRents pr where pr.ClientUserId = @ClientUserId order by pr.Date desc
end

create procedure [dbo].[GetLastPreRentItems]
	@PreRentId uniqueidentifier
as
begin
	select pRI.Id, m.Id as MovieId, pRI.RentDuration, m.Title, m.FirebasePosterId, m.Price
	from PreRentItems pRI 
	join Movies m on pRI.MovieId = m.Id
	where pRI.RentId = @PreRentId;
end