--
-- PostgreSQL database dump
--

-- Dumped from database version 13.8 (Debian 13.8-1.pgdg110+1)
-- Dumped by pg_dump version 14.4

-- Started on 2022-11-07 10:37:06 MSK

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
-- TOC entry 201 (class 1259 OID 16448)
-- Name: ticket; Type: TABLE; Schema: public; Owner: program
--

CREATE TABLE public.ticket (
    id integer NOT NULL,
    ticket_uid uuid NOT NULL,
    username character varying(80) NOT NULL,
    flight_number character varying(20) NOT NULL,
    price integer NOT NULL,
    status character varying(20) NOT NULL,
    CONSTRAINT ticket_status_check CHECK (((status)::text = ANY ((ARRAY['PAID'::character varying, 'CANCELED'::character varying])::text[])))
);


ALTER TABLE public.ticket OWNER TO program;

--
-- TOC entry 200 (class 1259 OID 16446)
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: public; Owner: program
--

CREATE SEQUENCE public.ticket_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ticket_id_seq OWNER TO program;

--
-- TOC entry 2996 (class 0 OID 0)
-- Dependencies: 200
-- Name: ticket_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: program
--

ALTER SEQUENCE public.ticket_id_seq OWNED BY public.ticket.id;


--
-- TOC entry 2853 (class 2604 OID 16451)
-- Name: ticket id; Type: DEFAULT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.ticket ALTER COLUMN id SET DEFAULT nextval('public.ticket_id_seq'::regclass);


--
-- TOC entry 2990 (class 0 OID 16448)
-- Dependencies: 201
-- Data for Name: ticket; Type: TABLE DATA; Schema: public; Owner: program
--

COPY public.ticket (id, ticket_uid, username, flight_number, price, status) FROM stdin;
1	11be4cb7-dbb9-4b5b-a299-8d9ce94f7a7c	test	AFL031	1500	CANCELED
\.


--
-- TOC entry 2997 (class 0 OID 0)
-- Dependencies: 200
-- Name: ticket_id_seq; Type: SEQUENCE SET; Schema: public; Owner: program
--

SELECT pg_catalog.setval('public.ticket_id_seq', 3, true);


--
-- TOC entry 2856 (class 2606 OID 16454)
-- Name: ticket ticket_pkey; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT ticket_pkey PRIMARY KEY (id);


--
-- TOC entry 2858 (class 2606 OID 16456)
-- Name: ticket ticket_ticket_uid_key; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT ticket_ticket_uid_key UNIQUE (ticket_uid);


-- Completed on 2022-11-07 10:37:06 MSK

--
-- PostgreSQL database dump complete
--

