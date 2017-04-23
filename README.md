In this application, I am reading through a descent size text file. This file could be written in .txt, or .log. It doesn't matter. There are no filters on the file types.

After reading the file using StreamReader, I am running each line through a RegEx Match.
I will then run a `where` clause looking for the Status = 200, and `RequestedIpAddress` that startes with `207.114`.
I group the result set by IpAddress, and retain the `Count()` as Quanity.
I created an extension method for Linq called `ToCSV()`, this method evaluates the prooerties, and their values. It will construct the correct string for the csv.
I save the csv to the root of the project.

Couple of notes to follow:
I decided to use an IEnumerable, instead of a DataTable, List, or Array due to size. The memory size captured by the Enumerator is far less than others. As well as the execution time for functions such as Count, Sum, and GroupBy.