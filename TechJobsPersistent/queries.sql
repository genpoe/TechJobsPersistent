--Part 1
SELECT Id, Name, EmployerId
FROM jobs;
--Id int AI PK
--Name longtext
--EmployerId int

--Part 2
SELECT *
FROM techjobs.employers
WHERE (Location = "St. Louis City");

--Part 3
SELECT DISTINCT name, description
FROM techjobs.skills
INNER JOIN techjobs.jobskills ON techjobs.skills.id	= techjobs.jobskills.skillid
ORDER BY name;
