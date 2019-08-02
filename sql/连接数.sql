SELECT @@MAX_CONNECTIONS


SELECT * FROM master.dbo.sysprocesses WHERE dbid IN
(
SELECT dbid FROM master.dbo.sysdatabases
WHERE NAME='[fhcollectionGM1]'
)
--根据需要更改YourDataBaseName
SELECT * FROM master.dbo.sysprocesses WHERE DB_NAME(dbid) = 'fhcollectionGM1'

SELECT * FROM sysprocesses

SELECT @@CONNECTIONS


SELECT ds.session_id,
       ds.status,
       Db_name(dr.database_id) AS database_name,
       ds.login_name,
       ds.login_time,
       ds.host_name,
       dc.client_net_address,
       dc.client_tcp_port,
       ds.program_name,
       dr.cpu_time,
       dr.reads,
       dr.writes,
       dc.num_reads,
       dc.num_writes,
       ds.client_interface_name,
       ds.last_request_start_time,
       ds.last_request_end_time,
       dc.connect_time,
       dc.net_transport,
       dc.net_packet_size,
       dr.start_time,
       dr.status,
       dr.command,
       dr.blocking_session_id,
       dr.wait_type,
       dr.wait_time,
       dr.last_wait_type,
       dr.wait_resource,
       dr.open_transaction_count,
       dr.percent_complete,
       dr.granted_query_memory
FROM   Sys.dm_exec_requests dr WITH(nolock)
       RIGHT OUTER JOIN Sys.dm_exec_sessions ds WITH(nolock)
                     ON dr.session_id = ds.session_id
       RIGHT OUTER JOIN Sys.dm_exec_connections dc WITH(nolock)
                     ON ds.session_id = dc.session_id
WHERE  ds.session_id > 50
ORDER  BY ds.program_name


SELECT login_name,
       Count(0) user_count
FROM   Sys.dm_exec_requests dr WITH(nolock)
       RIGHT OUTER JOIN Sys.dm_exec_sessions ds WITH(nolock)
                     ON dr.session_id = ds.session_id
       RIGHT OUTER JOIN Sys.dm_exec_connections dc WITH(nolock)
                     ON ds.session_id = dc.session_id
WHERE  ds.session_id > 50
GROUP  BY login_name
ORDER  BY user_count DESC
