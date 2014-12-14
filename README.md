TDS
===

Test Data Seeding

###SQL Data type compatibility:

Exact Numerics:

	bit			        compatible
	tinyint			    compatible
	smallint		    compatible
	int			        compatible
	bigint			    compatible
	decimal			    compatible
	numeric			    compatible
	smallmoney		    compatible
	money			    compatible
Approximate Numerics:

	real			    compatible
	float			    compatible
Date and Time:

	date			    compatible
	time			    compatible
	smalldatetime	    compatible
	datetime		    compatible
	datetime2		    compatible
	datetimeoffset	    compatible
Character Strings:

	char			    compatible
	varchar			    compatible
	text			    compatible
Unicode Character Strings:

	nchar			    compatible
	nvarchar		    compatible
	ntext			    compatible
Binary Strings:

	binary 			    incompatible    -binary type, to be handled
	varbinary		    incompatible    -binary type, to be handled
	image			    incompatible    -binary type, to be handled
Other Data Types:

	uniqueidentifier	compatible
	hierarchyid		    compatible
	sql_variant		    compatible
	xml			        compatbile	    -note: if not primary key
	geography		    compatible	    -note: if not primary key
	geometry		    compatible	    -note: if not primary key
	timestamp		    incompatible	-binary type, to be handled
	cursor			    -
	table			    -

