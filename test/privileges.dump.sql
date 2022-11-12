--
-- PostgreSQL database dump
--

-- Dumped from database version 13.8 (Debian 13.8-1.pgdg110+1)
-- Dumped by pg_dump version 14.4

-- Started on 2022-11-07 10:41:55 MSK

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 201 (class 1259 OID 16459)
-- Name: privilege; Type: TABLE; Schema: public; Owner: program
--

CREATE TABLE public.privilege (
    id integer NOT NULL,
    username character varying(80) NOT NULL,
    status character varying(80) DEFAULT 'BRONZE'::character varying NOT NULL,
    balance integer,
    CONSTRAINT privilege_status_check CHECK (((status)::text = ANY ((ARRAY['BRONZE'::character varying, 'SILVER'::character varying, 'GOLD'::character varying])::text[])))
);


ALTER TABLE public.privilege OWNER TO program;

--
-- TOC entry 203 (class 1259 OID 16471)
-- Name: privilege_history; Type: TABLE; Schema: public; Owner: program
--

CREATE TABLE public.privilege_history (
    id integer NOT NULL,
    privilege_id integer,
    ticket_uid uuid NOT NULL,
    datetime timestamp without time zone NOT NULL,
    balance_diff integer NOT NULL,
    operation_type character varying(20) NOT NULL,
    CONSTRAINT privilege_history_operation_type_check CHECK (((operation_type)::text = ANY ((ARRAY['FILL_IN_BALANCE'::character varying, 'DEBIT_THE_ACCOUNT'::character varying])::text[])))
);


ALTER TABLE public.privilege_history OWNER TO program;

--
-- TOC entry 202 (class 1259 OID 16469)
-- Name: privilege_history_id_seq; Type: SEQUENCE; Schema: public; Owner: program
--

CREATE SEQUENCE public.privilege_history_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.privilege_history_id_seq OWNER TO program;

--
-- TOC entry 3010 (class 0 OID 0)
-- Dependencies: 202
-- Name: privilege_history_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: program
--

ALTER SEQUENCE public.privilege_history_id_seq OWNED BY public.privilege_history.id;


--
-- TOC entry 200 (class 1259 OID 16457)
-- Name: privilege_id_seq; Type: SEQUENCE; Schema: public; Owner: program
--

CREATE SEQUENCE public.privilege_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.privilege_id_seq OWNER TO program;

--
-- TOC entry 3011 (class 0 OID 0)
-- Dependencies: 200
-- Name: privilege_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: program
--

ALTER SEQUENCE public.privilege_id_seq OWNED BY public.privilege.id;


--
-- TOC entry 2859 (class 2604 OID 16462)
-- Name: privilege id; Type: DEFAULT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege ALTER COLUMN id SET DEFAULT nextval('public.privilege_id_seq'::regclass);


--
-- TOC entry 2862 (class 2604 OID 16474)
-- Name: privilege_history id; Type: DEFAULT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege_history ALTER COLUMN id SET DEFAULT nextval('public.privilege_history_id_seq'::regclass);


--
-- TOC entry 3002 (class 0 OID 16459)
-- Dependencies: 201
-- Data for Name: privilege; Type: TABLE DATA; Schema: public; Owner: program
--

COPY public.privilege (id, username, status, balance) FROM stdin;
1	Test Max	GOLD	1500
\.


--
-- TOC entry 3004 (class 0 OID 16471)
-- Dependencies: 203
-- Data for Name: privilege_history; Type: TABLE DATA; Schema: public; Owner: program
--

COPY public.privilege_history (id, privilege_id, ticket_uid, datetime, balance_diff, operation_type) FROM stdin;
1	1	11be4cb7-dbb9-4b5b-a299-8d9ce94f7a7c	2021-10-08 19:59:19	1500	FILL_IN_BALANCE
\.


--
-- TOC entry 3012 (class 0 OID 0)
-- Dependencies: 202
-- Name: privilege_history_id_seq; Type: SEQUENCE SET; Schema: public; Owner: program
--

SELECT pg_catalog.setval('public.privilege_history_id_seq', 1, true);


--
-- TOC entry 3013 (class 0 OID 0)
-- Dependencies: 200
-- Name: privilege_id_seq; Type: SEQUENCE SET; Schema: public; Owner: program
--

SELECT pg_catalog.setval('public.privilege_id_seq', 1, true);


--
-- TOC entry 2869 (class 2606 OID 16477)
-- Name: privilege_history privilege_history_pkey; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege_history
    ADD CONSTRAINT privilege_history_pkey PRIMARY KEY (id);


--
-- TOC entry 2865 (class 2606 OID 16466)
-- Name: privilege privilege_pkey; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege
    ADD CONSTRAINT privilege_pkey PRIMARY KEY (id);


--
-- TOC entry 2867 (class 2606 OID 16468)
-- Name: privilege privilege_username_key; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege
    ADD CONSTRAINT privilege_username_key UNIQUE (username);


--
-- TOC entry 2870 (class 2606 OID 16478)
-- Name: privilege_history privilege_history_privilege_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.privilege_history
    ADD CONSTRAINT privilege_history_privilege_id_fkey FOREIGN KEY (privilege_id) REFERENCES public.privilege(id);


-- Completed on 2022-11-07 10:41:55 MSK

--
-- PostgreSQL database dump complete
--

