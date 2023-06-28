#run the setup script to create the DB and the schema in the DB
#do this in a loop because the timing for when the SQL instance is ready is indeterminate
for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d master -i /usr/src/app/dbsetup.sql
    if [ $? -eq 0 ]
    then
        echo "setup.sql completed"
        echo "Creating Schemas"
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/inklio.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/order_sequence.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/user.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/tag.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/ask.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/delivery.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/comment.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/flag.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/image.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/upvote.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/ask_tag.sql
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Supersecret!1 -d inklio -i /usr/src/app/inklio/Tables/delivery_tag.sql
        echo "Schemas created"
        sleep infinity
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done